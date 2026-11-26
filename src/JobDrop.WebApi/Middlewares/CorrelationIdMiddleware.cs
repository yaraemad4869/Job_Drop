using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JobDrop.WebApi.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
        {
            correlationId = System.Guid.NewGuid().ToString();
            context.Request.Headers["X-Correlation-Id"] = correlationId;
        }
        
        context.Response.Headers["X-Correlation-Id"] = correlationId;
        await _next(context);
    }
}
