using ChaCha.MediatR.DomainEvents;

namespace ChaCha.Core.Domain;

public interface IEntity
{
  IReadOnlyCollection<IDomainEvent> GetDomainEvents();
  void ClearDomainEvents();
  void OnPreCommit();
}