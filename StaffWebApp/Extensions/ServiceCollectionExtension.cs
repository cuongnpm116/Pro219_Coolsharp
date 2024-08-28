using StaffWebApp.Services.Implements;
using StaffWebApp.Services.Interfaces;

namespace StaffWebApp.Extensions;

internal static class ServiceCollectionExtension
{
    internal static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IStaffService, StaffService>();

        return services;
    }
}
