using StaffWebApp.Services.Category;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Order;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Role;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Staff;
using WebAppIntegrated.Grpc;

namespace StaffWebApp.Extensions;

internal static class ServiceCollectionExtension
{
    internal static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IGrpcService, GrpcService>();

        services.AddTransient<IStaffService, StaffService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ISizeService, SizeService>();
        services.AddTransient<IColorService, ColorService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IProductService, ProductService>();

        return services;
    }
}
