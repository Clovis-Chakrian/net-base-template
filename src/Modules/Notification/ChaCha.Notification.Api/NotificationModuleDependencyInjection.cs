using ChaCha.Bus;
using ChaCha.Data;
using ChaCha.MediatR.Extensions;
using ChaCha.Notification.Application.Email.Commands.Send;
using ChaCha.Notification.Application.Email.Services;
using ChaCha.Notification.Application.NotificationsSent.Integration;
using ChaCha.Notification.Application.WhatsApp.Services;
using ChaCha.Notification.Infra.Persistence;
using ChaCha.Notification.Infra.Repositories.NotificationSents;
using ChaCha.Notification.Infra.Services;
using ChaCha.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Notification.Api;

public static class NotificationModuleDependencyInjection
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? "Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678";
        services
            .AddMediator(typeof(SendEmailCommand).Assembly)
            .AddDataConfig<NotificationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            },
                    [
                        typeof(NotificationSentReadRepository).Assembly,
                    ]);

        // services.AddAuthClient();

        services.AddMessageBus();
        services.AddHostedService<SendEmailIntegrationEventsHandler>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IWhatsAppService, WhatsAppService>();
        services.AddStorage();

        return services;
    }
}