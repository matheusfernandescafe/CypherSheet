using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CypherSheet.Client;
using CypherSheet.Client.Services;
using CypherSheet.Shared;
using MudBlazor.Services;
using TG.Blazor.IndexedDB;
using CypherSheet.Storage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configurar MudBlazor
builder.Services.AddMudServices();

// Configurar IndexedDB
builder.Services.AddIndexedDB(CypherSheetDb.Configure);

// Registrar serviços de domínio e persistência
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

// Registrar serviços de UI e tema
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<ILoadingService, LoadingService>();
builder.Services.AddScoped<CypherSheet.Client.Services.INotificationService, NotificationService>();

// Registrar serviços de imagem
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageCacheService, ImageCacheService>();
builder.Services.AddScoped<IImageNotificationService, ImageNotificationService>();

// Registrar serviços de gerenciamento de dados
builder.Services.AddScoped<CypherSheet.Shared.IDataManagementService, DataManagementService>();
builder.Services.AddScoped<CypherSheet.Shared.ICacheService, CacheService>();

await builder.Build().RunAsync();
