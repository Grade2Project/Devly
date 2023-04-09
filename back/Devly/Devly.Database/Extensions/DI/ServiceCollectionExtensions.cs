using Devly.Database.Basics.Context;
using Devly.Database.Basics.Context.ContextProvider;
using Devly.Database.Basics.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Devly.Database.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNpgsqlDbContext<T>(
        this IServiceCollection services, 
        string connectionString,
        int poolSize = 1024) where T : EfDbContext
    {
        services.AddPooledDbContextFactory<T>(
            (opt => opt.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()),
            poolSize);
        
        
        services.AddEfDatabaseInternal<T>();
        services.AddDbContextFactory<T>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services;
    }


    private static IServiceCollection AddEfDatabaseInternal<TContext>(this IServiceCollection services)
        where TContext : EfDbContext
    {
        services.AddSingleton<IDbRepository<TContext>, EfDbRepository<TContext>>();
        services.AddSingleton<IDbContextProvider<TContext>, EfDbContextProvider<TContext>>();

        return services;
    }
}