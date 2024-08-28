using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;
internal static class JwtAuthenticationExtensions
{
    internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        string issuer = configuration.GetValue<string>("Jwt:Issuer")
            ?? throw new InvalidOperationException("The JWT Issuer value is not initialized yet.");
        string audience = configuration.GetValue<string>("Jwt:Audience")
            ?? throw new InvalidOperationException("The JWT Audience value is not initialized yet.");
        string key = configuration.GetValue<string>("Jwt:SecretKey")
            ?? throw new InvalidOperationException("The JWT Secret key value is not initialized yet.");
        byte[] signingKeyBytes = System.Text.Encoding.ASCII.GetBytes(key);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
            };
        });

        return services;
    }
}
