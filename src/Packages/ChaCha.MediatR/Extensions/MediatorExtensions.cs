using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ChaCha.MediatR.Commands;
using ChaCha.MediatR.DomainEvents;
using ChaCha.MediatR.Mediator;
using ChaCha.MediatR.Queries;

namespace ChaCha.MediatR.Extensions;

public static class MediatorExtensions
{
  public static IServiceCollection AddMediator(this IServiceCollection services, Assembly? assembly = null)
  {
    assembly ??= Assembly.GetCallingAssembly();

    services.AddScoped<IMediator, MediatorHandler>();

    var queryHandlerInterfaceType = typeof(IQueryHandler<,>);
    var queryHandlerInterfaceTypeNoArgs = typeof(IQueryHandler<>);
    var commandHandlerInterfaceType = typeof(ICommandHandler<,>);
    var commandHandlerInterfaceTypeNoArgs = typeof(ICommandHandler<>);
    var domainEventHandlerInterfaceType = typeof(IDomainEventHandler<>);

    var handlerTypes = assembly
        .GetTypes()
        .Where(type => !type.IsAbstract && !type.IsInterface)
        .SelectMany(type => type.GetInterfaces()
            .Where(i =>
              i.IsGenericType &&
              (
                i.GetGenericTypeDefinition() == queryHandlerInterfaceType ||
                i.GetGenericTypeDefinition() == commandHandlerInterfaceType ||
                i.GetGenericTypeDefinition() == queryHandlerInterfaceTypeNoArgs ||
                i.GetGenericTypeDefinition() == commandHandlerInterfaceTypeNoArgs ||
                i.GetGenericTypeDefinition() == domainEventHandlerInterfaceType
              )
            ).Select(i => new { Interface = i, Implementation = type }));

    foreach (var handler in handlerTypes)
    {
      services.AddScoped(handler.Interface, handler.Implementation);
    }

    return services;
  }
}