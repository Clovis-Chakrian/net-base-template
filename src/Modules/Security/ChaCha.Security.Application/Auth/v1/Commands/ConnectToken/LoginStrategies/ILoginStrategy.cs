using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;

public interface ILoginStrategy
{
  Task<SignInResult> HandleLogin(OpenIddictRequest request, CancellationToken cancellationToken);
}