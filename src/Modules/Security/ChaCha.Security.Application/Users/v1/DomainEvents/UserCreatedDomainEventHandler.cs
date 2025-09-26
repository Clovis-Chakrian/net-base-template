using Microsoft.Extensions.Logging;
using ChaCha.Security.Domain.Users.Events;
using ChaCha.MediatR.DomainEvents;
using ChaCha.Bus;
using EasyNetQ;
using ChaCha.IntegrationEvents.Users.Created;

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

  public void Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("User created with ID: {UserId} - Full Name: {FullName} - Email: {Email}", domainEvent.UserId, domainEvent.FullName, domainEvent.Email);
    var createdUserIntegrationEventObject = new CreatedUserIntegrationEventObject(
      userId: domainEvent.UserId,
      fullName: domainEvent.FullName,
      email: domainEvent.Email
      );

    var createdUserIntegrationEvent = new CreatedUserIntegrationEvent(createdUserIntegrationEventObject);
    
    _bus.PublishAsync<CreatedUserIntegrationEvent, CreatedUserIntegrationEventObject>(createdUserIntegrationEvent, cancellationToken);
  }
}