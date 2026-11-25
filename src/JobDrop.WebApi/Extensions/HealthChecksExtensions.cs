using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace JobDrop.WebApi.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddJobDropHealthChecks(this IHealthChecksBuilder builder, string connectionString)
    {
        builder.AddNpgSql(
            connectionString,
            name: "postgres",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "ready" });
        return builder;
    }

    public static WebApplication MapJobDropHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false,
            ResponseWriter = WriteResponse
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("ready"),
            ResponseWriter = WriteResponse
        });

        return app;
    }

    private static Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var payload = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            entries = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}