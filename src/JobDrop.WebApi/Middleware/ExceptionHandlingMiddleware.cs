using FluentValidation;
using System.Net;
using System.Text.Json;
using JobDrop.Domain.Exceptions;

namespace JobDrop.WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, errorCode, message) = exception switch
        {
            DomainRuleException dre => (HttpStatusCode.Conflict, dre.ErrorCode, dre.Message),
            NotFoundException nfe => (HttpStatusCode.NotFound, "NOT_FOUND", nfe.Message),
            ValidationException ve => (HttpStatusCode.BadRequest, "VALIDATION_ERROR", ve.Message),
            _ => (HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = new
            {
                code = errorCode,
                message = message,
                timestamp = DateTime.UtcNow
            }
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
