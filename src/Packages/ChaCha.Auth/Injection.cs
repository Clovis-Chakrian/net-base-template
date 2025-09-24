using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChaCha.Auth;

public static class Injection
{
  public static IServiceCollection AddAuthServer(this IServiceCollection services)
  {
    services.AddOpenIddict()

        // Register the OpenIddict server components.
        .AddServer(options =>
        {
          options
            .UseReferenceRefreshTokens()
            .SetRefreshTokenLifetime(TimeSpan.FromDays(180))
            .SetAccessTokenLifetime(TimeSpan.FromMinutes(15));

          // options.UseDataProtection();

          // Enable the token endpoint.
          options.SetTokenEndpointUris("connect/token");
          options.SetAuthorizationEndpointUris("connect/authorize");
          // Enable the client credentials flow.

          options.AllowClientCredentialsFlow();
          options.AllowAuthorizationCodeFlow();
          options.AllowRefreshTokenFlow();
          options.AllowPasswordFlow();

          // Register the signing and encryption credentials.
          options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

          options.DisableAccessTokenEncryption();

          // Register the ASP.NET Core host and configure the ASP.NET Core options.
          options.UseAspNetCore()
            .EnableTokenEndpointPassthrough();
        })
        .AddValidation(options =>
    {
      // Note: the validation handler uses OpenID Connect discovery
      // to retrieve the issuer signing keys used to validate tokens.
      options.SetIssuer("https://localhost:5137/");
      // options.UseDataProtection();
      // Register the encryption credentials. This sample uses a symmetric
      // encryption key that is shared between the server and the API project.
      //
      // Note: in a real world application, this encryption key should be
      // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
      options.AddEncryptionKey(new SymmetricSecurityKey(
          Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

      // Register the System.Net.Http integration.
      options.UseSystemNetHttp();

      // Register the ASP.NET Core host.
      options.UseAspNetCore();
    });

    return services;
  }

  // public static IServiceCollection AddAuthDbConfiguration(this IServiceCollection services)
  // {
  //   services.AddDbContext<TDbContext>(options =>
  //     {
  //        // Configure Entity Framework Core to use Microsoft SQL Server.
  //       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

  //        // Register the entity sets needed by OpenIddict.
  //        // Note: use the generic overload if you need to replace the default OpenIddict entities.
  //       options.UseOpenIddict();
  //     });

  //   services.AddOpenIddict()

  //       // Register the OpenIddict core components.
  //       .AddCore(options =>
  //       {
  //         // Configure OpenIddict to use the Entity Framework Core stores and models.
  //         // Note: call ReplaceDefaultEntities() to replace the default entities.
  //         options.UseEntityFrameworkCore()
  //           .UseDbContext<TDbContext>();
  //       });
  //   return services;
  // }
}