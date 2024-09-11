using MudBlazor.Services;
using StaffWebApp.Components;
using StaffWebApp.Extensions;
using StaffWebApp.Services.Voucher;
using WebAppIntegrated.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(opts => { opts.DetailedErrors = true; });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(ShopConstants.EShopClient, client =>
{
    client.BaseAddress = new Uri(ShopConstants.EShopApiHost);
});

builder.Services.AddDataServices();

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

await app.RunAsync();
