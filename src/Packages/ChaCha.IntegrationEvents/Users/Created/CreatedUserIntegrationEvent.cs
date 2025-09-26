using ChaCha.Bus.Base;

namespace ChaCha.IntegrationEvents.Users.Created;

public class CreatedUserIntegrationEvent : IntegrationEvent<CreatedUserIntegrationEventObject>
{
  public CreatedUserIntegrationEvent(CreatedUserIntegrationEventObject message)
    : base(message, UserIntegrationEventConstants.ExchangeName, UserIntegrationEventConstants.CreatedRoutingKey)
  {
  }
}