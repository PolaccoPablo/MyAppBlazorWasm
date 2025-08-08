using MiAppBlazorWasm.Web.Client;
using MiAppBlazorWasm.Web.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient con la URL base del servidor
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7000/") // Puerto del servidor API
});

// Registrar servicios
builder.Services.AddScoped<WeatherService>();

await builder.Build().RunAsync();