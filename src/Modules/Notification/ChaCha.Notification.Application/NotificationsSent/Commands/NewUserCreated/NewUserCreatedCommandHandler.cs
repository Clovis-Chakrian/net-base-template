using ChaCha.MediatR;
using ChaCha.MediatR.Commands;
using ChaCha.Notification.Domain.TokensSent;
using ChaCha.Notification.Domain.TokenTypes;
using ChaCha.Notification.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChaCha.Notification.Application.NotificationsSent.Commands.NewUserCreated;

public class NewUserCreatedCommandHandler : ICommandHandler<NewUserCreatedCommand>
{
  private readonly NotificationDbContext _context;
  private readonly ILogger<NewUserCreatedCommandHandler> _logger;

  public NewUserCreatedCommandHandler(NotificationDbContext context, ILogger<NewUserCreatedCommandHandler> logger)
  {
    _context = context;
    _logger = logger;
  }

  public async Task<Result> Handle(NewUserCreatedCommand command, CancellationToken cancellationToken)
  {
    var result = Result.Create();

    var tokenType = await _context.TokenTypes.FirstOrDefaultAsync(p => p.Id == ETokenType.MFA, cancellationToken);

    if (tokenType is null)
    {
      result.AddFailure("Token type not found.");
      return result;
    }

    var tokenResult = TokenSent.Create(
      tokenType: tokenType,
      dataCheck: command.Email
    );

    if (!tokenResult.IsValid)
    {
      result.AddFailure(tokenResult.ValidationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Error creating token.");
      return result;
    }

    var token = tokenResult.Data;
    _logger.LogInformation("Token created: {Token}", token?.Token);
    token?.OnPreCommit();
    _logger.LogInformation("Token encrypted: {EncryptedToken}", token?.Token);

    return result;
  }
}