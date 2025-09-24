using System.Security.Claims;
using ChaCha.Security.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;

public sealed class PasswordLoginStrategy : ILoginStrategy
{
  private readonly IOpenIddictApplicationManager _applicationManager;
  private readonly UserManager<User> _userManager;

  public PasswordLoginStrategy(IOpenIddictApplicationManager applicationManager, UserManager<User> userManager)
  {
    _applicationManager = applicationManager;
    _userManager = userManager;
  }
  public async Task<Microsoft.AspNetCore.Mvc.SignInResult> HandleLogin(OpenIddictRequest request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Username!);
    if (user == null) return null;

    var checkPasswordResult = await _userManager.CheckPasswordAsync(user, request.Password!);
    if (!checkPasswordResult) return null;

    var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);
    identity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id);
    identity.SetClaim(OpenIddictConstants.Claims.Name, user.FullName);
    identity.SetClaim(OpenIddictConstants.Claims.Email, user.Email);
    identity.SetClaim(OpenIddictConstants.Claims.EmailVerified, user.EmailConfirmed);

    identity.SetDestinations(static claim => claim.Type switch
      {
         // Allow the "name" claim to be stored in both the access and identity tokens
         // when the "profile" scope was granted (by calling principal.SetScopes(...)).
        OpenIddictConstants.Claims.Name when claim.Subject!.HasScope(OpenIddictConstants.Scopes.Profile)
                  => [OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken],

         // Otherwise, only store the claim in the access tokens.
        _ => [OpenIddictConstants.Destinations.AccessToken]
      });

    identity.SetScopes("openid", "profile", "api", "offline_access");
    
    return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
  }
}