using ChaCha.Auth.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace ChaCha.Auth;

public static class Injection
{
  public static IServiceCollection AddAuthServer<TUser, TDbContext>(this IServiceCollection services)
    where TUser : ApplicationUser
    where TDbContext : DbContext
  {
    services.AddIdentityCore<TUser>()
      .AddEntityFrameworkStores<TDbContext>();

    services.AddOpenIddict()
        .AddCore(options =>
        {
          options.UseEntityFrameworkCore()
            .UseDbContext<TDbContext>();
        })
        .AddServer(options =>
        {
          options.SetIssuer(new Uri("https://localhost:5137/"));

          options
            .UseReferenceAccessTokens()
            .UseReferenceRefreshTokens()
            .SetRefreshTokenLifetime(TimeSpan.FromDays(180))
            .SetAccessTokenLifetime(TimeSpan.FromMinutes(15));

          options.SetTokenEndpointUris("connect/token");
          options.SetIntrospectionEndpointUris("/connect/introspect");
          options.SetUserInfoEndpointUris("/connect/userinfo");
          options.SetAuthorizationEndpointUris("connect/authorize");

          options.AllowClientCredentialsFlow();
          options.AllowAuthorizationCodeFlow();
          options.AllowRefreshTokenFlow();
          options.AllowPasswordFlow();
          options.RegisterScopes(OpenIddictConstants.Scopes.OpenId, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Email, "api", OpenIddictConstants.Scopes.OfflineAccess);


          options.AddDevelopmentSigningCertificate();
          options.AddDevelopmentEncryptionCertificate();

          options.DisableAccessTokenEncryption();

          options.UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableUserInfoEndpointPassthrough();
        })
        .AddValidation(o =>
        {
          o.UseLocalServer(); 
          o.UseAspNetCore();   
        });

    services.AddAuthentication(o =>
      {
        o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
      });

    return services;
  }

  public static IServiceCollection AddAuthClient(this IServiceCollection services)
  {
    services.AddAuthentication(options =>
    {
      options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    });

    services
      .AddOpenIddict()
      .AddValidation(options =>
      {
        options.UseLocalServer();

        options.UseAspNetCore();
      })
      ;
    return services;
  }
}