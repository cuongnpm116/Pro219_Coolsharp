namespace Api.Extensions;
internal static class CorsPolicyExtensions
{
    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(opts =>
        {
            opts.AddPolicy("eShopApi", builder =>
            {
                builder.WithOrigins("https://localhost:2000", "https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
