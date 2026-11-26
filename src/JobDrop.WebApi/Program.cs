using JobDrop.Application;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Infrastructure;
using JobDrop.Infrastructure.Identity;
using JobDrop.Infrastructure.Persistence;
using JobDrop.Infrastructure.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting JobDrop WebApi...");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // =================================================================================================
    // Service Registration
    // =================================================================================================

    // Database
    

    // ASP.NET Core Identity
    builder.Services.AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // Application & Infrastructure Layers
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // Custom Application Services
    builder.Services.AddScoped<ITokenService, JwtTokenService>();
    builder.Services.AddScoped<ICurrentUser, CurrentUserService>();

    // Background Workers
    builder.Services.AddHostedService<SlaExpirationRefundWorker>();

    // Swagger / OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "JobDrop API",
            Version = "v1",
            Description = "API for the JobDrop platform - Sprint"
        });

        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // Controllers
    builder.Services.AddControllers();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Authentication & Authorization
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();


// =====================================================================
// MISSING SERVICE REGISTRATIONS (Added by Fixer)
// =====================================================================

// Domain Services
builder.Services.AddSingleton<JobDrop.Domain.Services.DropWalletPolicy>();
builder.Services.AddSingleton<JobDrop.Domain.Services.SlaDeadlineCalculator>();
builder.Services.AddSingleton<JobDrop.Domain.Services.MatchScoringEngine>();
builder.Services.AddSingleton<JobDrop.Domain.Services.CandidateCodeGenerator>();
builder.Services.AddSingleton<JobDrop.Domain.Services.BlindDossierProjector>();

// Infrastructure Services
builder.Services.AddTransient<JobDrop.Application.Contracts.Billing.IPaymobClient, JobDrop.Infrastructure.Billing.PaymobClient>();
builder.Services.AddTransient<JobDrop.Application.Contracts.Billing.IEntitlementResolver, JobDrop.Infrastructure.Billing.EntitlementResolver>();
builder.Services.AddTransient<JobDrop.Application.Contracts.Notifications.IEmailSender, JobDrop.Infrastructure.Notifications.EmailSender>();
builder.Services.AddTransient<JobDrop.Application.Contracts.Storage.IMalwareScanner, JobDrop.Infrastructure.Storage.MalwareScanner>();
builder.Services.AddTransient<JobDrop.Application.Contracts.Storage.IPrivateDocumentStorage, JobDrop.Infrastructure.Storage.PrivateDocumentStorage>();
builder.Services.AddTransient<JobDrop.Application.Contracts.Persistence.IOutboxPublisher, JobDrop.Infrastructure.Notifications.OutboxPublisher>();

// Background Workers
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.SlaExpirationRefundWorker>();
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.QueueDispatchWorker>();
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.OutboxNotificationWorker>();
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.PaymobReconcilerWorker>();
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.ReverseDropExpiryWorker>();
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.DocumentScanWorker>();
builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.AccountDeletionPurgeWorker>();

    var app = builder.Build();

    // =================================================================================================
    // Middleware Pipeline
    // =================================================================================================

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobDrop API V1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseMiddleware<JobDrop.WebApi.Middleware.ExceptionHandlingMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("JobDrop WebApi is ready.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
