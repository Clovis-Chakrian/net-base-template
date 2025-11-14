using ChaCha.Bus;
using ChaCha.IntegrationEvents.Notifications;
using ChaCha.IntegrationEvents.Notifications.Emails;
using ChaCha.MediatR.Mediator;
using ChaCha.Notification.Application.Email.Commands.Send;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.NotificationsSent.Integration;

public class SendEmailIntegrationEventsHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly ILogger<SendEmailIntegrationEventsHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const string ServiceName = "chacha.svc.notification";

    public SendEmailIntegrationEventsHandler(IMessageBus bus, ILogger<SendEmailIntegrationEventsHandler> logger, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ListenMessages();
        return Task.CompletedTask;
    }

    private void OnConnect(object s, EventArgs e)
    {
        ListenMessages();
    }

    private void ListenMessages()
    {
        _bus.SubscribeAsync<SendEmailIntegrationEventObject>(
            exchangeName: NotificationIntegrationEventConstants.ExchangeName,
            queueName: GetQueueName(NotificationIntegrationEventConstants.SendEmailRoutingKey),
            bindingKey: NotificationIntegrationEventConstants.SendEmailRoutingKey,
            onMessage: SendEmail
        );

        _bus.AdvancedBus.Connected += OnConnect!;
    }

    public async Task SendEmail(SendEmailIntegrationEventObject request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando o envio de email");

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new SendEmailCommand(
            TemplateId: request.TemplateId,
            Subject: request.Subject,
            Tos: request.Tos,
            Ccs: request.Ccs,
            Bccs: request.Bccs,
            TemplateParams: request.TemplateParams
        );

        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsValid)
        {
            _logger.LogError("Erro ao tentar enviar emails para {Email}. Error: {Error}", request.Tos, result.ValidationResult.Errors.FirstOrDefault()?.ErrorMessage);
            return;
        }

        _logger.LogInformation("Email enviado com sucesso para {Email}", request.Tos);
    }

    private static string GetQueueName(string routingKey) => $"{ServiceName}.{routingKey}.q";
}