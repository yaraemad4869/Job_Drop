using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobDrop.WebApi.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class RequireIdempotencyKeyAttribute : Attribute, IAsyncActionFilter
{
    public const string HeaderName = "Idempotency-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var keyValues))
        {
            context.Result = new BadRequestObjectResult(new
            {
                type = "urn:jobdrop:problem:missing-idempotency-key",
                title = $"Header '{HeaderName}' is required.",
                status = 400,
                code = "MISSING_IDEMPOTENCY_KEY"
            });
            return;
        }

        var key = keyValues.ToString();
        if (string.IsNullOrWhiteSpace(key) || key.Length > 100 || !System.Text.RegularExpressions.Regex.IsMatch(key, "^[a-zA-Z0-9_-]+$"))
        {
            context.Result = new BadRequestObjectResult(new
            {
                type = "urn:jobdrop:problem:invalid-idempotency-key",
                title = $"Header '{HeaderName}' must match ^[a-zA-Z0-9_-]+$ and be at most 100 chars.",
                status = 400,
                code = "INVALID_IDEMPOTENCY_KEY"
            });
            return;
        }

        context.HttpContext.Items["IdempotencyKey"] = key;
        await next();
    }
}