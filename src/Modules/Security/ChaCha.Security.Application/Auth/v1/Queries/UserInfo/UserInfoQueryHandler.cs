using ChaCha.MediatR;
using ChaCha.MediatR.Queries;
using ChaCha.Security.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Application.Auth.v1.Queries.UserInfo;

public class UserInfoQueryHandler : IQueryHandler<UserInfoQuery, IActionResult>
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserInfoQueryHandler(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Task<Result<IActionResult>> Handle(UserInfoQuery query, CancellationToken cancellationToken)
  {
    var result = Result<IActionResult>.Create();

    var user = _httpContextAccessor.HttpContext?.User;

    var sub = user!.GetClaim(OpenIddictConstants.Claims.Subject);
    var name = user!.GetClaim(OpenIddictConstants.Claims.Name);
    var email = user!.GetClaim(OpenIddictConstants.Claims.Email);
    var aud = user!.GetClaim(OpenIddictConstants.Claims.Audience);
    var iss = user!.GetClaim(OpenIddictConstants.Claims.Issuer);


    bool hasEmailScope = user!.HasScope(OpenIddictConstants.Scopes.Email);
    bool hasProfileScope = user!.HasScope(OpenIddictConstants.Scopes.Profile);

    var response = new Dictionary<string, object?>()
    {
      ["aud"] = aud,
      ["iss"] = iss,
      ["sub"] = sub,
    };

    if (hasProfileScope)
    {
      response.Add("name", name);
    }

    if (hasEmailScope)
    {
      response.Add("email", email);

      var emailVerified = user!.GetClaim(OpenIddictConstants.Claims.EmailVerified);

      response.Add("email_verified", emailVerified ?? "false");
    }

    return Task.FromResult(result.Success(new OkObjectResult(response)));
  }
}