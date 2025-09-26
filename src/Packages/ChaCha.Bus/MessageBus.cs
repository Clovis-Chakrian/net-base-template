using EasyNetQ;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace ChaCha.Bus;

#nullable disable
public class MessageBus : IMessageBus
{
  private IBus _bus;
  private IAdvancedBus _advancedBus;
  private readonly string _connectionString;
  public bool IsConnected => _advancedBus?.IsConnected ?? false;
  public IAdvancedBus AdvancedBus => _bus?.Advanced;

  public MessageBus(string connectionString)
  {
    _connectionString = connectionString;
    TryConnect();
  }

  public async Task PublishAsync<TEvent, TMessage, TMessageType>(TEvent integrationEvent, CancellationToken cancellationToken = default)
    where TEvent : IntegrationEvent<TMessage, TMessageType>
    where TMessage : Message<TMessageType>
  {
    TryConnect();

    var exchange = _advancedBus.ExchangeDeclare(integrationEvent.Exchange, ExchangeType.Topic, cancellationToken: cancellationToken);

    await _advancedBus.PublishAsync(
      exchange: exchange,
      routingKey: integrationEvent.RoutingKey,
      mandatory: false,
      message: integrationEvent.Message,
      cancellationToken: cancellationToken);
  }

  public async Task SubscribeAsync<TMessageType>(string exchangeName, string queueName, string bindingKey, Func<TMessageType, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default) where TMessageType : class
  {
    TryConnect();

    var queue = await _advancedBus
      .QueueDeclareAsync(
        name: queueName,
        cfg =>
        {
          cfg.AsDurable(true);
          cfg.AsAutoDelete(false);
        },
        cancellationToken: cancellationToken
      );

    var exchange = await _advancedBus.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, cancellationToken: cancellationToken);

    await _advancedBus.BindAsync(
      exchange: exchange,
      queue: queue,
      routingKey: bindingKey,
      arguments: null,
      cancellationToken: cancellationToken
      );

    _advancedBus.Consume<TMessageType>(
      queue,
      (message, info) => onMessage(message.Body, cancellationToken),
      cfg =>
      {
        cfg.WithAutoAck();
      }
      );
  }

  private void TryConnect()
  {
    if (IsConnected)
    {
      Console.WriteLine("Já conectado ao RabbitMQ, retornando...");
      return;
    }

    const int retryCount = 7;

    var policy = Policy.Handle<EasyNetQException>()
        .Or<BrokerUnreachableException>()
        .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (ex, ts, i, context) =>
            {
              Console.WriteLine($"Tentativa {i} do {retryCount} de conexão ao RabbitMQ");
            });
    try
    {
      policy.Execute(() =>
      {
        _bus = RabbitHutch.CreateBus(_connectionString);
        _advancedBus = _bus.Advanced;
        _advancedBus.Disconnected += OnDisconnect;
      });
    }
    catch (Exception ex)
    {
      var msg = $"Não foi possível se conectar ao RabbitMQ após {retryCount} tentativas. {ex.Message}";
      Console.WriteLine(msg);
      throw new EasyNetQException(msg);
    }
  }

  private void OnDisconnect(object s, EventArgs e)
  {
    var policy = Policy.Handle<EasyNetQException>()
        .Or<BrokerUnreachableException>()
        .RetryForever();

    policy.Execute(TryConnect);
  }

  public void Dispose()
  {
    _advancedBus.Dispose();
  }
}