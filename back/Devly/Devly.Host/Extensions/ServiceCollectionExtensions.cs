namespace Devly.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfig<T>(this IServiceCollection services, IConfigurationSection section) 
        where T : class, new()
    {
        var config = new T();
        section.Bind(config);
        services.AddSingleton(config);

        return services;
    }
}