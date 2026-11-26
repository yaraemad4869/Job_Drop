using Microsoft.AspNetCore.Http;

namespace JobDrop.WebApi.Middleware;

/// <summary>
/// Validates Idempotency-Key header for POST endpoints that require it.
/// </summary>
public sealed class IdempotencyKeyMiddleware
{
    private readonly RequestDelegate _next;

    public IdempotencyKeyMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        var requiresIdempotency = path.Contains("/drops") && context.Request.Method == "POST"
                               || path.Contains("/reverse-drops") && context.Request.Method == "POST";

        if (requiresIdempotency && !context.Request.Headers.ContainsKey("Idempotency-Key"))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync("{\"type\":\"urn:jobdrop:problem:missing-idempotency-key\",\"title\":\"Idempotency-Key header is required\",\"status\":400}");
            return;
        }

        await _next(context);
    }
}