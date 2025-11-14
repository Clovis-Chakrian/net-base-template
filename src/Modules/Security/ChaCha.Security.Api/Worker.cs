using ChaCha.Security.Infra.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Api;

public class Worker : IHostedService
{
  private readonly IServiceProvider _serviceProvider;

  public Worker(IServiceProvider serviceProvider)
      => _serviceProvider = serviceProvider;

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    using var scope = _serviceProvider.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();
    await context.Database.EnsureCreatedAsync();

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    var authmanager = scope.ServiceProvider.GetRequiredService<IOpenIddictAuthorizationManager>();


    if (await manager.FindByClientIdAsync("service-worker") is null)
    {
      await manager.CreateAsync(new OpenIddictApplicationDescriptor
      {
        ClientId = "service-worker",
        ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
        ClientType = OpenIddictConstants.ClientTypes.Confidential,
        DisplayName = "Service Worker (Password Flow)",

        Permissions =
        {
            // Endpoints que este client poderá chamar
            OpenIddictConstants.Permissions.Endpoints.Token,
          OpenIddictConstants.Permissions.Prefixes.Endpoint + "userinfo",   // << necessário para /connect/userinfo
            OpenIddictConstants.Permissions.Endpoints.Revocation, // se você usa revogação (opcional)

            // Grant types
            OpenIddictConstants.Permissions.GrantTypes.Password,
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

            // Scopes OIDC + o seu "api"
            OpenIddictConstants.Permissions.Prefixes.Scope + "openid",
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Prefixes.Scope + "offline_access",
            OpenIddictConstants.Permissions.Prefixes.Scope + "api"
        }
      });
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}