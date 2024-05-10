namespace NFTBlockchain.Domain
{
    using Apps;
    using HostedServices;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Bundle
    {
        public static IServiceCollection UseDomainServices(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddHostedService<MigratorHostedService>();
            services.AddSingleton<NFTApp>(_ => new NFTApp());
            
            return services;
        } 
    }
}