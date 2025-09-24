using ChaCha.MediatR;
using ChaCha.MediatR.Commands;
using ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Security.Application.Auth.v1.Commands.ConnectToken;

public class ConnectTokenCommandHandler : ICommandHandler<ConnectTokenCommand, SignInResult>
{
  private readonly IServiceProvider _serviceProvider;
  public ConnectTokenCommandHandler(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public async Task<Result<SignInResult>> Handle(ConnectTokenCommand command, CancellationToken cancellationToken)
  {
    var result = Result<SignInResult>.Create();
    var loginStrategy = _serviceProvider.GetKeyedService<ILoginStrategy>(command.OpenIddictRequest.GrantType);

    if (loginStrategy is null)
    {
      result.AddFailure($"No login strategy found for grant type {command.OpenIddictRequest.GrantType}");
      return result;
    }
    
    var loginResult = await loginStrategy.HandleLogin(command.OpenIddictRequest, cancellationToken);

    return result.Success(loginResult);
  }
}