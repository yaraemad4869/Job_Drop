using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JobDrop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // Register all FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);
        
        // Register MediatR (if not already registered in Program.cs)
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        return services;
    }
}
