using JobDrop.Application.Common;
using Microsoft.AspNetCore.Http;

namespace JobDrop.Infrastructure.Identity;

public sealed class RequestContextService : IRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CorrelationId
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx is null) return Guid.CreateVersion7().ToString();
            if (ctx.Items.TryGetValue("CorrelationId", out var obj) && obj is string s && !string.IsNullOrEmpty(s))
                return s;
            var generated = Guid.CreateVersion7().ToString();
            ctx.Items["CorrelationId"] = generated;
            return generated;
        }
    }

    public string? IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    public string? UserAgent => _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
}