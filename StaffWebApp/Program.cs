using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MudBlazor.Services;
using StaffWebApp.Components;
using StaffWebApp.Extensions;
using WebAppIntegrated.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(opts => { opts.DetailedErrors = true; });

builder.Services.AddControllers();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient(ShopConstants.EShopClient, client =>
{
    client.BaseAddress = new Uri(ShopConstants.EShopApiHost);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(opts =>
{
    opts.Cookie.Name = "auth_token";
    opts.LoginPath = "/login";
    opts.Cookie.MaxAge = TimeSpan.FromMinutes(30);
    opts.AccessDeniedPath = "/access-denied";
    opts.SlidingExpiration = true;
    opts.Events.OnValidatePrincipal = async context =>
    {
        if (context.Principal == null)
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync();
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

builder.Services.AddDataServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
