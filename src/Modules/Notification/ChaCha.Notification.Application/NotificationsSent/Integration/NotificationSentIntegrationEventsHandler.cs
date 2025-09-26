using ChaCha.Bus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.NotificationsSent.Integration;

public class NotificationSentIntegrationEventsHandler : BackgroundService
{

  private readonly IMessageBus _bus;
  private readonly ILogger<NotificationSentIntegrationEventsHandler> _logger;

  public NotificationSentIntegrationEventsHandler(IMessageBus bus, ILogger<NotificationSentIntegrationEventsHandler> logger)
  {
    _bus = bus;
    _logger = logger;
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
    _bus.SubscribeAsync<string>("user.events", "svc.notification.user.created.q", "user.created", SendEmailNewUserCreated);

    _bus.AdvancedBus.Connected += OnConnect!;
  }

  public Task SendEmailNewUserCreated(string request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Message received: {Message}", request);
    return Task.CompletedTask;
  }
}