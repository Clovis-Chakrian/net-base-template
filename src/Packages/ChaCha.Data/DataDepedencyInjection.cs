using System.Reflection;
using ChaCha.Core.Domain;
using ChaCha.Core.Repositories;
using ChaCha.Data.Persistence.Repositories.Cache;
using ChaCha.Data.Persistence.Repositories.Read;
using ChaCha.Data.Persistence.Repositories.Write;
using ChaCha.Data.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChaCha.Data;

public static class DataDepedencyInjection
{
    public static IServiceCollection AddDataConfig<TDbContext>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder>? dbContextOptions,
        Assembly[] assemblies)
    where TDbContext : DbContext
    {
        services
            .AddSingleton<ICacheRepository, CacheRepository>()
            .AddDbContext<TDbContext>(dbContextOptions)
            .AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>()
            .AddRepositories(assemblies);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
        {
            assemblies = new[] { typeof(DataDepedencyInjection).Assembly };
        }

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IReadRepository<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IWriteRepository<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}