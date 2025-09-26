using ChaCha.Bus;
using ChaCha.Data.Persistence.UnitOfWork;
using ChaCha.MediatR.Extensions;
using ChaCha.Notification.Application.NotificationsSent.Commands.NewUserCreated;
using ChaCha.Notification.Application.NotificationsSent.Integration;
using ChaCha.Notification.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Notification.Api;

public static class NotificationModuleDependencyInjection
{
  public static IServiceCollection AddNotificationModule(this IServiceCollection services)
  {
    services.AddMediator(typeof(NewUserCreatedCommand).Assembly);
    services.AddScoped<IUnitOfWork, UnitOfWork<NotificationDbContext>>();
    services.AddDbContext<NotificationDbContext>(options =>
    {
      options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ??
        "Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678");
    });
    
    services.AddMessageBus();
    services.AddHostedService<NotificationSentIntegrationEventsHandler>();

    return services;
  }
}