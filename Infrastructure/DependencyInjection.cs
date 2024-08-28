using Application.Abstractions;
using Application.IRepositories;
using Application.IServices;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using VietNamAddress.Models;
using VietNamAddress.Repos;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddDbContext<VietNamAddressContext>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IStorageService, FileStorageService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IVietNamAddressRepository, VietNamAddressRepository>();
        services.AddTransient<IAddressRepository, AddressRepository>();

        return services;
    }
}
