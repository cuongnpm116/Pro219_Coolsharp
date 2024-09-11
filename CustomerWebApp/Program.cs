using CustomerWebApp.Components;
using CustomerWebApp.Extensions;
using MudBlazor;
using MudBlazor.Services;
using WebAppIntegrated.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDataServices();

builder.Services.AddHttpClient(ShopConstants.EShopClient, client =>
{
    client.BaseAddress = new Uri(ShopConstants.EShopApiHost);
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
