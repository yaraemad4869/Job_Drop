using System.Text;
using JobDrop.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

namespace JobDrop.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobDropAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        services.Configure<JwtOptions>(o =>
        {
            o.Issuer = jwtOptions.Issuer;
            o.Audience = jwtOptions.Audience;
            o.SecretKey = jwtOptions.SecretKey;
            o.AccessTokenLifetimeMinutes = jwtOptions.AccessTokenLifetimeMinutes;
            o.RefreshTokenLifetimeDays = jwtOptions.RefreshTokenLifetimeDays;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.FromSeconds(10)
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorizationBuilder()
            .AddPolicy("JobSeeker", p => p.RequireRole("JobSeeker"))
            .AddPolicy("Recruiter", p => p.RequireRole("CompanyOwner", "CompanyMember"))
            .AddPolicy("CompanyOwner", p => p.RequireRole("CompanyOwner"))
            .AddPolicy("PlatformAdmin", p => p.RequireRole("PlatformAdmin"));

        return services;
    }

    public static IServiceCollection AddJobDropRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter("LoginBucket", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(15);
                o.QueueLimit = 0;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.AddSlidingWindowLimiter("SearchSlidingWindow", o =>
            {
                o.PermitLimit = 60;
                o.Window = TimeSpan.FromMinutes(1);
                o.SegmentsPerWindow = 6;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.AddSlidingWindowLimiter("DropSlidingWindow", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(1);
                o.SegmentsPerWindow = 4;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.AddConcurrencyLimiter("ExportConcurrencyLimit", o =>
            {
                o.PermitLimit = 1;
                o.QueueLimit = 0;
            });
        });

        return services;
    }

    public static IServiceCollection AddJobDropSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "JobDrop API",
                Version = "v1",
                Description = "Spatial Career Dispatch & Recruitment Platform API",
                Contact = new OpenApiContact { Name = "JobDrop Engineering", Email = "engineering@jobdrop.local" }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }
}