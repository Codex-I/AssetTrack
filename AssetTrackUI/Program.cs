using AssetTrackUI;
using AssetTrackUI.Core.API;
using AssetTrackUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//var settings = new Settings();
//builder.Configuration.Bind(settings);

//builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<AlertService>();
builder.Services.AddSingleton<SpinnerService>();

//builder.Services.AddScoped<Client>();
var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7031/") };
var apiClient = new Client(httpClient);
builder.Services.AddScoped(z => apiClient);
builder.Services.AddScoped(sp => httpClient);

await builder.Build().RunAsync();
