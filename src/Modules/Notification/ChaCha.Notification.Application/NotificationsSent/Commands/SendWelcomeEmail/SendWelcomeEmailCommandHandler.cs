using ChaCha.MediatR;
using ChaCha.MediatR.Commands;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.NotificationsSent.Commands.SendWelcomeEmail;

public class SendWelcomeEmailCommandHandler : ICommandHandler<SendWelcomeEmailCommand>
{
  private readonly ILogger<SendWelcomeEmailCommandHandler> _logger;

  public SendWelcomeEmailCommandHandler(ILogger<SendWelcomeEmailCommandHandler> logger)
  {
    _logger = logger;
  }

  public Task<Result> Handle(SendWelcomeEmailCommand command, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Sending welcome email to {Email} - {FullName}", command.Email, command.FullName);
    return Task.FromResult(Result.Create().Success());
  }
}