using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;

public sealed class ClientCredentialsLoginStrategy : ILoginStrategy
{
  private readonly IOpenIddictApplicationManager _applicationManager;

  public ClientCredentialsLoginStrategy(IOpenIddictApplicationManager applicationManager)
  {
    _applicationManager = applicationManager;
  }
  public async Task<SignInResult> HandleLogin(OpenIddictRequest request, CancellationToken cancellationToken)
  {
    // Note: the client credentials are automatically validated by OpenIddict:
    // if client_id or client_secret are invalid, this action won't be invoked.

    var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
        throw new InvalidOperationException("The application cannot be found.");

    // Create a new ClaimsIdentity containing the claims that
    // will be used to create an id_token, a token or a code.
    var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

    // Use the client_id as the subject identifier.
    identity.SetClaim(OpenIddictConstants.Claims.Subject, await _applicationManager.GetClientIdAsync(application));
    identity.SetClaim(OpenIddictConstants.Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

    identity.SetDestinations(static claim => claim.Type switch
    {
      // Allow the "name" claim to be stored in both the access and identity tokens
      // when the "profile" scope was granted (by calling principal.SetScopes(...)).
      OpenIddictConstants.Claims.Name when claim.Subject!.HasScope(OpenIddictConstants.Scopes.Profile)
                => [OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken],

      // Otherwise, only store the claim in the access tokens.
      _ => [OpenIddictConstants.Destinations.AccessToken]
    });

    var props = new Microsoft.AspNetCore.Authentication.AuthenticationProperties();
    props.AllowRefresh = true;

    return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), props);
  }
}