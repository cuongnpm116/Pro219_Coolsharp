using StaffWebApp.Service.Category;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Order;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Role;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Staff;
using StaffWebApp.Services.Voucher;

namespace StaffWebApp.Extensions;

internal static class ServiceCollectionExtension
{
    internal static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IStaffService, StaffService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ISizeService, SizeService>();
        services.AddTransient<IColorService, ColorService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IVoucherSevice, VoucherService>();

        return services;
    }
}
