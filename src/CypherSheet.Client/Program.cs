using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CypherSheet.Client;
using MudBlazor.Services;
using TG.Blazor.IndexedDB;
using CypherSheet.Storage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddIndexedDB(CypherSheetDb.Configure);
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

await builder.Build().RunAsync();
