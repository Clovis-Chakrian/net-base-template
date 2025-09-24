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
        Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Token,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                }
      });
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}