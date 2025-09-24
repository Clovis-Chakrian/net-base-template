using Microsoft.Extensions.Logging;
using ChaCha.Security.Domain.Users.Events;
using ChaCha.MediatR.DomainEvents;

namespace ChaCha.Security.Application.Users.v1.DomainEvents;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedEvent>
{
  private readonly ILogger<UserCreatedDomainEventHandler> _logger;

  public UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger)
  {
    _logger = logger;
  }

  public void Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("User created with ID: {UserId} - Full Name: {FullName} - Email: {Email}", domainEvent.UserId, domainEvent.FullName, domainEvent.Email);
    // Additional logic can be added here, such as sending a welcome email or logging the event.
  }
}