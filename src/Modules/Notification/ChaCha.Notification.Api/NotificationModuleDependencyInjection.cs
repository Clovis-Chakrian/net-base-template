using ChaCha.Bus;
using ChaCha.Data;
using ChaCha.MediatR.Extensions;
using ChaCha.Notification.Application.NotificationsSent.Commands.SendWelcomeEmail;
using ChaCha.Notification.Application.NotificationsSent.Integration;
using ChaCha.Notification.Domain.NotificationsSent.Repositories;
using ChaCha.Notification.Domain.TokenTypes.Repositories;
using ChaCha.Notification.Infra.Persistence;
using ChaCha.Notification.Infra.Repositories.NotificationSents;
using ChaCha.Notification.Infra.Repositories.TokenTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Notification.Api;

public static class NotificationModuleDependencyInjection
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? "Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678";
        services
            .AddMediator(typeof(SendWelcomeEmailCommand).Assembly)
            .AddDataConfig<NotificationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            },
                    [
                        typeof(NotificationSentReadRepository).Assembly,
                    ]);

        services.AddMessageBus();
        services.AddHostedService<NotificationSentIntegrationEventsHandler>();

        return services;
    }
}