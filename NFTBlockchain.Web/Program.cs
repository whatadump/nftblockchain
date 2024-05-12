using BlazorDownloadFile;
using NFTBlockchain.Infrastructure;
using NFTBlockchain.Infrastructure.Entities;
using NFTBlockchain.Web.Client;
using NFTBlockchain.Web.Client.Layout;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using NFTBlockchain.Web.Components;
using NFTBlockchain.Web.Components.Account;
using NFTBlockchain.Web.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using NFTBlockchain.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.UseDomainServices(builder.Configuration);
builder.Services.UseInfrastructureServices(builder.Configuration);
builder.Services.UseInteractiveApplication(builder.Configuration);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddBlazorDownloadFile();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<IdentityErrorDescriberRu>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
    
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(o =>
    {
        o.DetailedErrors = builder.Environment.IsDevelopment();
    });


var app = builder.Build();

Application.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Configuration.GetRequiredSection("Blockchain:NFTStorageFolder").Value),
    RequestPath = "/Artworks"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Configuration.GetRequiredSection("Blockchain:PrivateKeysTemp").Value),
    RequestPath = "/PrivateKeysTemp",
    ServeUnknownFileTypes = true
});
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MainLayout).Assembly);

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("ru-RU")
});

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
