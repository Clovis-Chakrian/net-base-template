using EasyNetQ;

namespace ChaCha.Bus.Base;

public abstract class IntegrationEvent<TMessage> where TMessage : class
{
  public string Exchange { get; private set; }
  public string RoutingKey { get;  private set; }
  public Message<TMessage> Message { get; private set; }

  protected IntegrationEvent(TMessage message, string exchange, string routingKey)
  {
    Exchange = exchange;
    RoutingKey = routingKey;
    Message = new Message<TMessage>(message);
  }
}