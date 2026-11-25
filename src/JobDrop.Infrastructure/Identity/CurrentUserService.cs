using System.Security.Claims;
using JobDrop.Application.Common;
using Microsoft.AspNetCore.Http;

namespace JobDrop.Infrastructure.Identity;

public sealed class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var sub = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue("sub");
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);
    public string? Handle => User?.FindFirstValue("handle");
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public IReadOnlyCollection<string> Roles =>
        User?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
        ?? (IReadOnlyCollection<string>)Array.Empty<string>();

    public bool IsInRole(string role) => Roles.Contains(role);
}