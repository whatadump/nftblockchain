namespace NFTBlockchain.Web.Client;

using Services;

public static class Bundle
{
    public static IServiceCollection UseInteractiveApplication(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddScoped<UserManager>();
        
        return services;
    }
}