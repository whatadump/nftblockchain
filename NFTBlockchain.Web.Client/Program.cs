using NFTBlockchain.Domain;
using NFTBlockchain.Infrastructure;
using NFTBlockchain.Web.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.UseInfrastructureServices(builder.Configuration);
builder.Services.UseDomainServices(builder.Configuration);


builder.Services.UseInteractiveApplication(builder.Configuration);

await builder.Build().RunAsync();
