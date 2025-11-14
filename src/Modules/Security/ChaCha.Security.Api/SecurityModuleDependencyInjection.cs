using ChaCha.Auth;
using ChaCha.Bus;
using ChaCha.Data;
using ChaCha.MediatR.Extensions;
using ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;
using ChaCha.Security.Application.Users.v1.Commands.Create;
using ChaCha.Security.Domain.Users;
using ChaCha.Security.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Api;

public static class SecurityModuleDependencyInjection
{
  public static IServiceCollection AddSecurityModuleDependencyInjection(this IServiceCollection services)
  {
    services.AddMediator(typeof(CreateUserCommandHandler).Assembly);
    services.AddDataConfig<SecurityDbContext>(options =>
    {
      options.UseOpenIddict();
      options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ??
        "Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678");
    }, []);

    services.AddMessageBus();
    services.AddHostedService<Worker>();
    services.AddAuthServer<User, SecurityDbContext>();

    services.AddKeyedScoped<ILoginStrategy, ClientCredentialsLoginStrategy>(OpenIddictConstants.GrantTypes.ClientCredentials);
    services.AddKeyedScoped<ILoginStrategy, PasswordLoginStrategy>(OpenIddictConstants.GrantTypes.Password);
    return services;
  }
}