using Microsoft.AspNetCore.Http;

namespace JobDrop.WebApi.Middleware;

public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(HeaderName))
        {
            context.Request.Headers[HeaderName] = Guid.CreateVersion7().ToString();
        }
        var correlationId = context.Request.Headers[HeaderName].ToString();
        context.Items["CorrelationId"] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });
        await _next(context);
    }
}