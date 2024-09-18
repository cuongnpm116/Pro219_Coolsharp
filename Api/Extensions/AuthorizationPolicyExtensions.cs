using Microsoft.AspNetCore.Authorization;

namespace Api.Extensions;

public static class AuthorizationPolicyExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("StaffOnly", policy => policy.RequireRole("staff"))
            .AddPolicy("AdminOnly", policy => policy.RequireRole("admin"))
            .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

        return services;
    }
}
