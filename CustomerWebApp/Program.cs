using CustomerWebApp.Components;
using CustomerWebApp.Components.Carts;
using CustomerWebApp.Extensions;
using CustomerWebApp.Service.Address;
using CustomerWebApp.Service.Cart;
using CustomerWebApp.Service.Category;
using CustomerWebApp.Service.Order;
using CustomerWebApp.Service.Payment;
using CustomerWebApp.Service.Product;
using MudBlazor;
using MudBlazor.Services;
using WebAppIntegrated.AddressService;
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

builder.Services.AddTransient<IVietNamAddressService, VietNamAddressService>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IAddressService, AddressService>();

builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<SelectedProductState>();

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
