using System.Text;
using JobDrop.Application;
using JobDrop.Application.Common;
using JobDrop.Application.Common.Behaviors;
using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Services;
using JobDrop.Infrastructure;
using JobDrop.Infrastructure.Billing;
using JobDrop.Infrastructure.Identity;
using JobDrop.Infrastructure.Persistence;
using JobDrop.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting JobDrop Web API");

    var builder = WebApplication.CreateBuilder(args);

    // 1. Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // 2. Problem Details & Exception Handling
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddHttpContextAccessor();

    // 3. Identity
    builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 10;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // 4. JWT Authentication
    var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
    if (string.IsNullOrEmpty(jwtSecretKey) || jwtSecretKey.Length < 32)
    {
        throw new InvalidOperationException("Jwt:SecretKey must be configured and at least 32 characters long");
    }

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "JobDrop",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "JobDrop",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

    // 5. Authorization Policies
    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("Candidate", policy => policy.RequireRole("Candidate"))
        .AddPolicy("Recruiter", policy => policy.RequireRole("Recruiter", "CompanyAdmin"))
        .AddPolicy("CompanyOwner", policy => policy.RequireRole("CompanyAdmin"))
        .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

    // 6. CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                ?? new[] { "http://localhost:3000", "http://localhost:5173" };
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
    });

    // 7. Rate Limiting
    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("fixed", limiterOptions =>
        {
            limiterOptions.PermitLimit = 100;
            limiterOptions.Window = TimeSpan.FromMinutes(1);
        });
        options.OnRejected = async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.HttpContext.Response.WriteAsJsonAsync(new { detail = "Rate limit exceeded." }, cancellationToken);
        };
    });

    // 8. MediatR with Behaviors
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(JobDrop.Application.DependencyInjection).Assembly);
        cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        cfg.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
    });

    // 9. Infrastructure & Application DI
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddSingleton<IDateTimeProvider, JobDrop.Infrastructure.SystemDateTimeProvider>();
    builder.Services.AddApplication();

    // 10. Real Services (NO MOCKS)
    builder.Services.AddScoped<ICurrentUser, CurrentUserService>();
    builder.Services.AddScoped<ITokenService, JwtTokenService>();
    builder.Services.AddScoped<IPaymobClient, PaymobClient>();
    builder.Services.AddScoped<DropWalletPolicy>();

    // 11. Application Services
    builder.Services.AddScoped<JobDrop.Application.Services.NeutralAvatarService>();
    builder.Services.AddScoped<JobDrop.Application.Services.ConversationService>();
    builder.Services.AddScoped<JobDrop.Application.Services.TalentSearchService>();
    builder.Services.AddScoped<JobDrop.Application.Services.IntakeSchedulerService>();
    builder.Services.AddScoped<JobDrop.Application.Services.MultiModeCommuteService>();
    builder.Services.AddScoped<JobDrop.Application.Services.CompanyRankingService>();
    builder.Services.AddScoped<JobDrop.Application.Services.LocationPrivacyService>();
    builder.Services.AddScoped<JobDrop.Application.Services.DossierPreviewService>();
    builder.Services.AddScoped<JobDrop.Application.Features.Billing.Services.RefundSafetyService>();
    builder.Services.AddScoped<JobDrop.Application.Features.Candidates.Services.BlindModeService>();

    // 12. Background Workers (Registered to prevent startup errors)
    // builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.QueueDispatchWorker>();
    // builder.Services.AddHostedService<JobDrop.Infrastructure.BackgroundJobs.SlaExpirationRefundWorker>();

    // 13. Controllers, OpenAPI, Health Checks
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();

    var app = builder.Build();

    // Middleware Pipeline (Order matters!)
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
    app.UseSerilogRequestLogging();
    app.UseCors("AllowFrontend");
    app.UseRateLimiter();
    
    app.UseMiddleware<CorrelationIdMiddleware>();
    
    app.UseExceptionHandler(); // Uses GlobalExceptionHandler
    
    app.UseAuthentication();
    app.UseMiddleware<TokenRevocationMiddleware>();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health/live");
    app.MapHealthChecks("/health/ready");

    Log.Information("Application started successfully");
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

