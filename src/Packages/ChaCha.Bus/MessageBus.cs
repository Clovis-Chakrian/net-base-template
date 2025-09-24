using EasyNetQ;

namespace ChaCha.Bus;

public class MessageBus
{
  private readonly IBus _bus;

  public MessageBus(IBus bus)
  {
    _bus = bus;
  }
  public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent
  {
    return _bus.PubSub.PublishAsync(
      message: integrationEvent,
      configure: cfg => cfg
        .WithTopic(integrationEvent.GetType().GetProperty("Topic")?.GetValue(null)?.ToString() ?? ""),
        cancellationToken);
    // .WithExchange(integrationEvent.GetType().GetProperty("Exchange")?.GetValue(null)?.ToString() ?? ""));
  }

  public Task SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default) where T : IntegrationEvent
  {
    return _bus.PubSub.SubscribeAsync(
      subscriptionId: subscriptionId,
      onMessage: onMessage,
      configure: cfg => cfg
        .WithTopic(typeof(T).GetProperty("Topic")?.GetValue(null)?.ToString() ?? "")
        .WithQueueName(typeof(T).GetProperty("QueueName")?.GetValue(null)?.ToString() ?? "")
        .WithExchangeType(""),
        cancellationToken);
    // .WithExchange(typeof(T).GetProperty("Exchange")?.GetValue(null)?.ToString() ?? ""));
  }


  // Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
}