using EasyNetQ;

namespace ChaCha.Bus;

public abstract class IntegrationEvent<TMessage, TMessageType> where TMessage : Message<TMessageType>
{
  public string Exchange { get; private set; }
  public string RoutingKey { get;  private set; }
  public TMessage Message { get; private set; }

  protected IntegrationEvent(TMessage message, string exchange, string routingKey)
  {
    Exchange = exchange;
    RoutingKey = routingKey;
    Message = message;
  }
}