namespace NFTBlockchain.Infrastructure
{
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Options;

    public static class Bundle
    {
        public static IServiceCollection UseInfrastructureServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    options => options.MigrationsAssembly("NFTBlockchain.Migrations")));

            services.AddSingleton<NFTFileOptions>(_ => new NFTFileOptions
            {
                BlockchainFilename = configuration.GetRequiredSection("Blockchain:Filepath").Value,
                NFTFileDirectory = configuration.GetRequiredSection("Blockchain:NFTStorageFolder").Value
            });
            
            return services;
        }
    }
}