using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Peliculas.Client;
using Peliculas.Client.Auth;
using Peliculas.Client.Repositorios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

ConfigureServices(builder.Services);

await builder.Build().RunAsync();

void ConfigureServices(IServiceCollection services) {
    services.AddSweetAlert2();
    services.AddScoped<IRepositorio, Repositorio>();
    //services.AddSingleton<IRepositorio, Repositorio>();
    services.AddAuthorizationCore();

    //services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

    services.AddScoped<ProveedorAutenticacionJWT>();
    services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>(proveedor => proveedor.GetRequiredService<ProveedorAutenticacionJWT>());
    services.AddScoped<ILoginService, ProveedorAutenticacionJWT>(proveedor => proveedor.GetRequiredService<ProveedorAutenticacionJWT>());
}   