using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using JobDrop.Application.Common.Exceptions;

namespace JobDrop.WebApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Status = (int)GetStatusCode(exception),
            Title = GetTitle(exception),
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7807"
        };

        // Add correlation ID if available
        if (httpContext.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
        {
            problemDetails.Extensions["correlationId"] = correlationId.ToString();
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static HttpStatusCode GetStatusCode(Exception exception) => exception switch
    {
        ValidationException => HttpStatusCode.BadRequest,
        UnauthorizedAccessException => HttpStatusCode.Forbidden,
        KeyNotFoundException => HttpStatusCode.NotFound,
        InvalidOperationException => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError
    };

    private static string GetTitle(Exception exception) => exception switch
    {
        ValidationException => "Validation Error",
        UnauthorizedAccessException => "Forbidden",
        KeyNotFoundException => "Not Found",
        InvalidOperationException => "Conflict",
        _ => "Server Error"
    };
}

