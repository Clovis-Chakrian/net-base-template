using ChaCha.Bus.Base;

namespace ChaCha.IntegrationEvents.Notifications.Emails;

public class SendEmailIntegrationEvent : IntegrationEvent<SendEmailIntegrationEventObject>
{
    public SendEmailIntegrationEvent(SendEmailIntegrationEventObject message) : base(message: message, exchange: NotificationIntegrationEventConstants.ExchangeName, routingKey: NotificationIntegrationEventConstants.SendEmailRoutingKey)
    {
    }
}