namespace NFTBlockchain.Domain
{
    using Apps;
    using Cryptography;
    using HostedServices;
    using Infrastructure.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class Bundle
    {
        public static IServiceCollection UseDomainServices(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddHostedService<MigratorHostedService>();
            services.AddSingleton<NFTApp>(_ => new NFTApp());

            services.AddSingleton<IEncryptor, RSAEncryptor>();
            services.AddSingleton<IHashFunction, SHA256Hash>();
            services.AddTransient<IArtworkService, ArtworkService>();
            
            return services;
        } 
    }
}