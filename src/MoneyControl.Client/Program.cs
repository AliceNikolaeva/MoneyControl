using Blazored.Modal;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoneyControl.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredModal();
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
    options.UserOptions.RoleClaim = "role";
});

builder.Services.AddHttpClient("moneycontrol",
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler(provider =>
    {
        var handler = provider.GetService<AuthorizationMessageHandler>()
            ?.ConfigureHandler(new[] { builder.HostEnvironment.BaseAddress }, new[] { "moneycontrolapi" });
        return handler;
    });
builder.Services.AddScoped(provider => provider.GetService<IHttpClientFactory>()?.CreateClient("moneycontrol"));

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();