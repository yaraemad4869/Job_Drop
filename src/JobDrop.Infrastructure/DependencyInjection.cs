using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobDrop.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            
        // Register the interface mapping
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        // Register DateTime Provider
        services.AddScoped<IDateTimeProvider, JobDrop.Application.Common.SystemDateTimeProvider>();

        
        return services;
    }
}
