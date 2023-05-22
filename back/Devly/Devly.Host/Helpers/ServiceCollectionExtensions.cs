namespace Devly.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<IFileHelper, FileHelper>();

        return services;
    }
}