using Microsoft.Extensions.Logging;
using ChaCha.Security.Domain.Users.Events;
using ChaCha.MediatR.DomainEvents;
using ChaCha.Bus;
using EasyNetQ;
using ChaCha.IntegrationEvents.Users.Created;
using ChaCha.IntegrationEvents.Notifications.Emails;
using System.Threading.Tasks;

namespace ChaCha.Security.Application.Users.v1.DomainEvents;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedEvent>
{
  private readonly ILogger<UserCreatedDomainEventHandler> _logger;
  private readonly IMessageBus _bus;

  public UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger, IMessageBus bus)
  {
    _logger = logger;
    _bus = bus;
  }

  public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("User created with ID: {UserId} - Full Name: {FullName} - Email: {Email}", domainEvent.UserId, domainEvent.FullName, domainEvent.Email);

    var sendEmailIntegrationEventObject = new SendEmailIntegrationEventObject(
      templateId: Guid.Parse("75762fed-89e0-4c98-9e3f-acad86361c1b"),
      subject: "Bem vindo!",
      tos: new List<string> { domainEvent.Email },
      templateParams: new Dictionary<string, string>
      {
        ["UserName"] = domainEvent.FullName,
        ["PlatformLink"] = "https://youtube.com",
        ["CancelInscriptionLink"] = "https://youtube.com"
      }
    );

    var sendEmailIntegrationEvent = new SendEmailIntegrationEvent(sendEmailIntegrationEventObject);

    await _bus.PublishAsync<SendEmailIntegrationEvent, SendEmailIntegrationEventObject>(sendEmailIntegrationEvent, cancellationToken);
  }
}