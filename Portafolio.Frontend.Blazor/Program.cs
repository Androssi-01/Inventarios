using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portafolio.Frontend.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{

    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost5205")

});

await builder.Build().RunAsync();
