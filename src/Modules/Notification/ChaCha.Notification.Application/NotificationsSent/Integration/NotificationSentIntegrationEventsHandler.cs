using ChaCha.Bus;
using ChaCha.IntegrationEvents.Users;
using ChaCha.IntegrationEvents.Users.Created;
using ChaCha.MediatR.Mediator;
using ChaCha.Notification.Application.NotificationsSent.Commands.SendWelcomeEmail;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.NotificationsSent.Integration;

public class NotificationSentIntegrationEventsHandler : BackgroundService
{
  private readonly IMessageBus _bus;
  private readonly ILogger<NotificationSentIntegrationEventsHandler> _logger;
  private readonly IServiceProvider _serviceProvider;
  private const string ServiceName = "chacha.svc.notification";

  public NotificationSentIntegrationEventsHandler(IMessageBus bus, ILogger<NotificationSentIntegrationEventsHandler> logger, IServiceProvider serviceProvider)
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
    _bus.SubscribeAsync<CreatedUserIntegrationEventObject>(
      exchangeName: UserIntegrationEventConstants.ExchangeName,
      queueName: GetQueueName(UserIntegrationEventConstants.CreatedRoutingKey),
      bindingKey: UserIntegrationEventConstants.CreatedRoutingKey,
      onMessage: SendEmailNewUserCreated
      );

    _bus.AdvancedBus.Connected += OnConnect!;
  }

  public async Task SendEmailNewUserCreated(CreatedUserIntegrationEventObject request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("New user created. User email: {UserEmail} - UserName {UserName}", request.Email, request.FullName);
    
    using var scope = _serviceProvider.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    var result = await mediator.Send(
      new SendWelcomeEmailCommand(
        Email: request.Email,
        FullName: request.FullName
        ),
        cancellationToken);

    if (!result.IsValid)
    {
      _logger.LogError("Error sending welcome email to {Email}. Error: {Error}", request.Email, result.ValidationResult.Errors.FirstOrDefault()?.ErrorMessage);
      return;
    }

    _logger.LogInformation("Welcome email sent to {Email}", request.Email);
  }

  private static string GetQueueName(string routingKey) => $"{ServiceName}.{routingKey}.q";
}