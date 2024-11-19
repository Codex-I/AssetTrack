using AssetTrackUI;
using AssetTrackUI.Core.API;
using AssetTrackUI.Features.HttpClientHandlers;
using AssetTrackUI.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add singleton services
builder.Services.AddSingleton<AlertService>();
builder.Services.AddSingleton<SpinnerService>();

// Add HTTP client handlers
builder.Services.AddTransient<AuthDelegateHandler>();
builder.Services.AddTransient<CacheDelegateHandler>();
builder.Services.AddTransient<SpinnerDelegateHandler>();

// Add authorization and local storage
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

// Add authentication state provider (test implementation)
builder.Services.AddScoped<AuthenticationStateProvider, TestAuthenticationStateProvider>();

// Set up HttpClient and API client
var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7031/") };
var apiClient = new Client(httpClient);
builder.Services.AddScoped(z => apiClient);
builder.Services.AddScoped(sp => httpClient);

await builder.Build().RunAsync();
