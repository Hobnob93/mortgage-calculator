using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MortgageCalculator;
using MortgageCalculator.Components.Interfaces;
using MortgageCalculator.Components.Services;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Services;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;
builder.Services.Configure<ApiEndpointConfig>(config.GetSection(ApiEndpointConfig.Section));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
});

builder.Services.AddTransient<IWebApiRequest, WebApiRequest>();
builder.Services.AddTransient<IIconFinder, IconFinder>();

builder.Services.AddHttpClient();

await builder.Build().RunAsync();
