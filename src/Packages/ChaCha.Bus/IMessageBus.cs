using ChaCha.Bus.Base;
using EasyNetQ;

namespace ChaCha.Bus;

public interface IMessageBus
{
  bool IsConnected { get; }
  IAdvancedBus AdvancedBus { get; }
  Task PublishAsync<TEvent, TMessage>(TEvent integrationEvent, CancellationToken cancellationToken = default) where TEvent : IntegrationEvent<TMessage> where TMessage : class;
  Task SubscribeAsync<TMessage>(string exchangeName, string queueName, string bindingKey, Func<TMessage, CancellationToken, Task> onMessage, CancellationToken cancellationToken = default) where TMessage : class;
  void Publish<TEvent, TMessage>(TEvent integrationEvent, CancellationToken cancellationToken = default) where TEvent : IntegrationEvent<TMessage> where TMessage : class;
  void Subscribe<TMessage>(string exchangeName, string queueName, string bindingKey, Func<TMessage, CancellationToken> onMessage, CancellationToken cancellationToken = default) where TMessage : class;
}