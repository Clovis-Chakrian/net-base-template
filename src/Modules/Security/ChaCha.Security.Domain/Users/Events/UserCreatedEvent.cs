using ChaCha.MediatR.DomainEvents;

namespace ChaCha.Security.Domain.Users.Events;

public record UserCreatedEvent(Guid UserId, string FullName, string Email) : IDomainEvent
{
}