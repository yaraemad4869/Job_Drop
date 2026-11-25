using System.Net;
using System.Text.Json;
using FluentValidation;
using JobDrop.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.CreateVersion7().ToString();
        var traceId = context.TraceIdentifier;

        HttpStatusCode status;
        string type;
        string title;
        string code;
        object? extensions = null;

        switch (exception)
        {
            case ValidationException validation:
                status = HttpStatusCode.BadRequest;
                type = "urn:jobdrop:problem:validation";
                title = "One or more validation errors occurred.";
                code = "VALIDATION_FAILED";
                extensions = new { errors = validation.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()) };
                break;

            case NotFoundException nf:
                status = HttpStatusCode.NotFound;
                type = "urn:jobdrop:problem:not-found";
                title = "Resource not found.";
                code = nf.ErrorCode;
                break;

            case ForbiddenAccessException:
                status = HttpStatusCode.Forbidden;
                type = "urn:jobdrop:problem:forbidden";
                title = "You do not have permission to perform this action.";
                code = "FORBIDDEN";
                break;

            case DomainRuleException dr:
                status = HttpStatusCode.Conflict;
                type = "urn:jobdrop:problem:domain-rule";
                title = dr.Message;
                code = dr.ErrorCode;
                if (dr.Errors.Count > 0) extensions = new { errors = dr.Errors };
                break;

            case ConcurrencyConflictException cc:
                status = HttpStatusCode.Conflict;
                type = "urn:jobdrop:problem:concurrency";
                title = "Concurrency conflict.";
                code = cc.ErrorCode;
                break;

            case InsufficientBalanceException:
                status = HttpStatusCode.PaymentRequired;
                type = "urn:jobdrop:problem:insufficient-balance";
                title = exception.Message;
                code = "INSUFFICIENT_BALANCE";
                break;

            case CapacityExceededException:
                status = HttpStatusCode.Conflict;
                type = "urn:jobdrop:problem:capacity-exceeded";
                title = exception.Message;
                code = "CAPACITY_EXCEEDED";
                break;

            case IdempotencyConflictException ic:
                status = HttpStatusCode.Conflict;
                type = "urn:jobdrop:problem:idempotency";
                title = exception.Message;
                code = "IDEMPOTENCY_CONFLICT";
                break;

            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                type = "urn:jobdrop:problem:unauthorized";
                title = "You are not authenticated.";
                code = "UNAUTHORIZED";
                break;

            default:
                _logger.LogError(exception, "Unhandled exception on {Path}", context.Request.Path);
                status = HttpStatusCode.InternalServerError;
                type = "urn:jobdrop:problem:internal";
                title = "An unexpected error occurred.";
                code = "INTERNAL_ERROR";
                break;
        }

        var problem = new Dictionary<string, object?>
        {
            ["type"] = type,
            ["title"] = title,
            ["status"] = (int)status,
            ["instance"] = context.Request.Path.Value,
            ["traceId"] = traceId,
            ["code"] = code,
            ["correlationId"] = correlationId
        };
        if (extensions is not null) problem["extensions"] = extensions;

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}