using EasyNetQ;

namespace ChaCha.Bus;

public interface IMessageBus
{
  bool IsConnected { get; }
  IAdvancedBus AdvancedBus { get; }
  Task PublishAsync<TEvent, TMessage, TMessageType>(TEvent integrationEvent, CancellationToken cancellationToken = default) where TEvent : IntegrationEvent<TMessage, TMessageType> where TMessage : Message<TMessageType>;
  Task SubscribeAsync<TMessageType>(string exchangeName, string queueName, string bindingKey, Func<TMessageType, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default) where TMessageType : class;
  public void Dispose();
}