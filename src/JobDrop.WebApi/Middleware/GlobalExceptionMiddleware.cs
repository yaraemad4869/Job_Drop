using JobDrop.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JobDrop.WebApi.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainRuleException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation");
            await WriteProblemDetails(context, StatusCodes.Status409Conflict, "urn:jobdrop:problem:domain-rule", "Domain rule violation", ex.Message, "DOMAIN_RULE_VIOLATION");
        }
        catch (NotFoundException ex)
        {
            await WriteProblemDetails(context, StatusCodes.Status404NotFound, "urn:jobdrop:problem:not-found", "Resource not found", ex.Message, "RESOURCE_NOT_FOUND");
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteProblemDetails(context, StatusCodes.Status403Forbidden, "urn:jobdrop:problem:forbidden", "Access denied", ex.Message, "ACCESS_DENIED");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblemDetails(context, StatusCodes.Status500InternalServerError, "urn:jobdrop:problem:internal-error", "Internal server error", "An unexpected error occurred.", "INTERNAL_ERROR");
        }
    }

    private static async Task WriteProblemDetails(HttpContext context, int statusCode, string type, string title, string detail, string code)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };
        problem.Extensions["code"] = code;
        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}