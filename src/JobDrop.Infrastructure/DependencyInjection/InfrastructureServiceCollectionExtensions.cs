using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Geography;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Contracts.Notifications;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Contracts.Storage;
using JobDrop.Infrastructure.Billing;
using JobDrop.Infrastructure.Geography;
using JobDrop.Infrastructure.Identity;
using JobDrop.Infrastructure.Notifications;
using JobDrop.Infrastructure.Persistence;
using JobDrop.Infrastructure.Persistence.Interceptors;
using JobDrop.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobDrop.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DomainEventsDispatcherInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsql.CommandTimeout(60);
                npgsql.EnableRetryOnFailure(3);
            });
            options.AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<DomainEventsDispatcherInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<IRequestContext, RequestContextService>();
        services.AddScoped<IOperatingAreaQuery, SqlSpatialOperatingAreaQuery>();
        services.AddScoped<ICommuteRouter, InMemoryCommuteRouter>();
        services.AddSingleton<IPrivateDocumentStorage, LocalFileStorage>();
        services.AddSingleton<IFileSignatureValidator, FileSignatureValidator>();
        services.AddScoped<IOutboxPublisher, OutboxPublisher>();
        services.AddSingleton<IEmailSender, ConsoleEmailSender>();
        services.AddSingleton<IWhatsAppSender, ConsoleWhatsAppSender>();
        services.AddScoped<IEntitlementResolver, DefaultEntitlementResolver>();

        return services;
    }
}