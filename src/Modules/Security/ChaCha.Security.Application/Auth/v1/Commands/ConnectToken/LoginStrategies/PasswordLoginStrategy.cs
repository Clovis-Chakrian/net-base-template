using System.Security.Claims;
using ChaCha.Security.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;

public sealed class PasswordLoginStrategy : ILoginStrategy
{
  private readonly UserManager<User> _userManager;

  public PasswordLoginStrategy(UserManager<User> userManager)
  {
    _userManager = userManager;
  }
  public async Task<IActionResult> HandleLogin(OpenIddictRequest request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByEmailAsync(request.Username!);
    if (user is null) return ForbidInvalidGrant("Invalid Grant");

    if (!await _userManager.CheckPasswordAsync(user, request.Password!))
      return ForbidInvalidGrant("Invalid Grant");

    var identity = new ClaimsIdentity(
        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
        nameType: OpenIddictConstants.Claims.Name,
        roleType: OpenIddictConstants.Claims.Role);

    identity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
    identity.SetClaim(OpenIddictConstants.Claims.Name, user.FullName ?? user.UserName ?? "");
    identity.SetClaim(OpenIddictConstants.Claims.Email, user.Email ?? "");
    identity.SetClaim(OpenIddictConstants.Claims.EmailVerified, user.EmailConfirmed);

    var principal = new ClaimsPrincipal(identity);

    var requested = request.GetScopes();
    var allowed = new[]
    {
            OpenIddictConstants.Scopes.OpenId, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.OfflineAccess, "api"
        };

    principal.SetScopes(requested.Intersect(allowed));

    foreach (var claim in principal.Claims)
    {
      claim.SetDestinations(GetDestinations(claim, principal));
    }

    return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
  }

  private static IActionResult ForbidInvalidGrant(string description)
  {
    return new ForbidResult(
      authenticationSchemes: new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
      properties: new AuthenticationProperties(new Dictionary<string, string?>
      {
        ["Error"] = OpenIddictConstants.Errors.InvalidGrant,
        ["ErrorDescription"] = description
      }));
  }

  private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
  {
    if (claim.Type == OpenIddictConstants.Claims.Name && principal.HasScope(OpenIddictConstants.Scopes.Profile))
      return new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken };

    if (claim.Type == OpenIddictConstants.Claims.Email && principal.HasScope(OpenIddictConstants.Scopes.Email))
      return new[] { OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken };

    if (claim.Type == OpenIddictConstants.Claims.Role)
      return new[] { OpenIddictConstants.Destinations.AccessToken };

    return new[] { OpenIddictConstants.Destinations.AccessToken };
  }
}