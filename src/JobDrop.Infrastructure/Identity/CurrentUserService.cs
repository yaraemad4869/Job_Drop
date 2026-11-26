using System.Security.Claims;
using JobDrop.Application.Common;
using Microsoft.AspNetCore.Http;

namespace JobDrop.Infrastructure.Identity;

public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? Handle => _httpContextAccessor.HttpContext?.User?.FindFirst("handle")?.Value;

    public IReadOnlyCollection<string> Roles
    {
        get
        {
            var roles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
            return roles?.AsReadOnly() ?? new List<string>().AsReadOnly();
        }
    }

    public Guid? CandidateProfileId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("candidateProfileId");
            if (claim != null && Guid.TryParse(claim.Value, out var id))
            {
                return id;
            }
            return null;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }
}
