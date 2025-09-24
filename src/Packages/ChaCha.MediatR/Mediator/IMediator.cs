using ChaCha.MediatR.Commands;
using ChaCha.MediatR.DomainEvents;
using ChaCha.MediatR.Queries;

namespace ChaCha.MediatR.Mediator;

public interface IMediator
{
  Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
  Task<Result> Send(ICommand command, CancellationToken cancellationToken = default);
  Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
  Task<Result> Send(IQuery query, CancellationToken cancellationToken = default);
  public void PublishEvent(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}