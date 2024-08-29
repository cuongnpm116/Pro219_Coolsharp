using StaffWebApp.Services.Role;
using StaffWebApp.Services.Staff;

namespace StaffWebApp.Extensions;

internal static class ServiceCollectionExtension
{
    internal static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IStaffService, StaffService>();
        services.AddTransient<IRoleService, RoleService>();

        return services;
    }
}
