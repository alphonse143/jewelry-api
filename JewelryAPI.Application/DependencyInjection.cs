using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using JewelryAPI.Application.Interfaces;
using JewelryAPI.Application.Services;

namespace JewelryAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
