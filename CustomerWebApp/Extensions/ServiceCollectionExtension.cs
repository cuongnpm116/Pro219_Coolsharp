using CustomerWebApp.Components.Carts;
using CustomerWebApp.Service.Address;
using CustomerWebApp.Service.Cart;
using CustomerWebApp.Service.Category;
using CustomerWebApp.Service.Customer;
using CustomerWebApp.Service.Order;
using CustomerWebApp.Service.Payment;
using CustomerWebApp.Service.Product;
using CustomerWebApp.Service.Voucher;
using WebAppIntegrated.AddressService;
using WebAppIntegrated.SignalR;

namespace CustomerWebApp.Extensions;

internal static class ServiceCollectionExtension
{
    internal static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IVietNamAddressService, VietNamAddressService>();

        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ICartService, CartService>();
        services.AddTransient<IPaymentService, PaymentService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IAddressService, AddressService>();
        services.AddTransient<IVoucherService, VoucherService>();

        services.AddScoped<CartState>();
        services.AddSingleton<SignalRService>();
        services.AddScoped<SelectedProductState>();


        return services;
    }
}
