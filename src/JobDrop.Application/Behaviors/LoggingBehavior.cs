using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobDrop.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).Name;
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogDebug("Handling {Request}", name);
            var response = await next();
            _logger.LogDebug("Handled {Request} in {ElapsedMs}ms", name, sw.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Request} after {ElapsedMs}ms", name, sw.ElapsedMilliseconds);
            throw;
        }
    }
}