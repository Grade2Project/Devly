namespace Devly.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<IFileHelper, FileHelper>();
        services.AddSingleton<IPhotoHelper, PhotoHelper>();

        return services;
    }
}