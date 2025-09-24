using Microsoft.Extensions.Logging;
using ChaCha.MediatR.Commands;
using ChaCha.MediatR.DomainEvents;
using ChaCha.MediatR.Queries;

namespace ChaCha.MediatR.Mediator;

internal class MediatorHandler(IServiceProvider serviceProvider, ILogger<MediatorHandler> logger) : IMediator
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  private readonly ILogger<MediatorHandler> _logger = logger;

  public Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Sending command of type {CommandType}", command.GetType().Name);
    var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
    dynamic? handler = _serviceProvider.GetService(handlerType);

    if (handler == null)
    {
      _logger.LogError("Command of Type {CommandType} not found! Did you miss some implementation?", command.GetType().Name);
      throw new InvalidOperationException($"No handler found for command of type {command.GetType().Name}");
    }

    var result = handler.Handle((dynamic)command, cancellationToken);
    if (result is Task<Result<TResponse>> taskResult)
    {
      return taskResult;
    }

    _logger.LogError("Handler for command of type {CommandType} returned invalid result", command.GetType().Name);
    throw new InvalidOperationException($"Handler for command of type {command.GetType().Name} returned invalid result");
  }

  public Task<Result> Send(ICommand command, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Sending command of type {CommandType}", command.GetType().Name);
    var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
    dynamic? handler = _serviceProvider.GetService(handlerType);

    if (handler == null)
    {
      _logger.LogError("Command of Type {CommandType} not found! Did you miss some implementation?", command.GetType().Name);
      throw new InvalidOperationException($"No handler found for command of type {command.GetType().Name}");
    }

    var result = handler.Handle((dynamic)command, cancellationToken);
    if (result is Task<Result> taskResult)
    {
      return taskResult;
    }

    _logger.LogError("Handler for command of type {CommandType} returned invalid result", command.GetType().Name);
    throw new InvalidOperationException($"Handler for command of type {command.GetType().Name} returned invalid result");
  }

  public Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Sending query of type {QueryType}", query.GetType().Name);
    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
    dynamic? handler = _serviceProvider.GetService(handlerType);

    if (handler == null)
    {
      _logger.LogError("Query of Type {QueryType} not found! Did you miss some implementation?", query.GetType().Name);
      throw new InvalidOperationException($"No handler found for query of type {query.GetType().Name}");
    }


    var result = handler.Handle((dynamic)query, cancellationToken);
    if (result is Task<Result<TResponse>> taskResult)
    {
      return taskResult;
    }

    _logger.LogError("Handler for query of type {QueryType} returned invalid result", query.GetType().Name);
    throw new InvalidOperationException($"Handler for query of type {query.GetType().Name} returned invalid result");
  }

  public Task<Result> Send(IQuery query, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Sending query of type {QueryType}", query.GetType().Name);
    var handlerType = typeof(IQueryHandler<>).MakeGenericType(query.GetType());
    dynamic? handler = _serviceProvider.GetService(handlerType);

    if (handler == null)
    {
      _logger.LogError("Query of Type {QueryType} not found! Did you miss some implementation?", query.GetType().Name);
      throw new InvalidOperationException($"No handler found for query of type {query.GetType().Name}");
    }


    var result = handler.Handle((dynamic)query, cancellationToken);
    if (result is Task<Result> taskResult)
    {
      return taskResult;
    }

    _logger.LogError("Handler for query of type {QueryType} returned invalid result", query.GetType().Name);
    throw new InvalidOperationException($"Handler for query of type {query.GetType().Name} returned invalid result");
  }

  public void PublishEvent(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Sending domain event of type {DomainEvent}", domainEvent.GetType().Name);
    var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
    dynamic? handler = _serviceProvider.GetService(handlerType);

    if (handler == null)
    {
      _logger.LogError("Domain Event of Type {DomainEvent} not found! Did you miss some implementation?", domainEvent.GetType().Name);
      throw new InvalidOperationException($"No handler found for domain event of type {domainEvent.GetType().Name}");
    }

    _logger.LogInformation("Calling handler for domain event of type {DomainEvent}", domainEvent.GetType().Name);
    handler.Handle((dynamic)domainEvent, cancellationToken);
  }
}