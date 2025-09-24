using ChaCha.Auth;
using ChaCha.Data.Persistence.UnitOfWork;
using ChaCha.MediatR.Extensions;
using ChaCha.Security.Application.Auth.v1.Commands.ConnectToken.LoginStrategies;
using ChaCha.Security.Application.Users.v1.Commands.Create;
using ChaCha.Security.Domain.Users;
using ChaCha.Security.Infra.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace ChaCha.Security.Api;

public static class SecurityModuleDependencyInjection
{
  public static IServiceCollection AddSecurityModuleDependencyInjection(this IServiceCollection services)
  {
    services.AddMediator(typeof(CreateUserCommandHandler).Assembly);
    services.AddScoped<IUnitOfWork, UnitOfWork<SecurityDbContext>>();
    services.AddDbContext<SecurityDbContext>(options =>
    {
      options.UseOpenIddict();
      options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ??
        "Server=localhost; Database=ModularMonolithDb; User Id=user; Password=12345678");
    });

    services.AddIdentityCore<User>()
      // .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<SecurityDbContext>();

    services.AddHostedService<Worker>();
    services.AddOpenIddict()
      .AddCore(options =>
      {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default entities.
        options.UseEntityFrameworkCore()
              .UseDbContext<SecurityDbContext>();
      });

    services.AddAuthServer();
    
    services.AddKeyedScoped<ILoginStrategy, ClientCredentialsLoginStrategy>(OpenIddictConstants.GrantTypes.ClientCredentials);
    services.AddKeyedScoped<ILoginStrategy, PasswordLoginStrategy>(OpenIddictConstants.GrantTypes.Password);
    return services;
  }
}