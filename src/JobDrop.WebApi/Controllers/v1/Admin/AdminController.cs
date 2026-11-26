using JobDrop.Application.Features.Admin.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Admin;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "PlatformAdmin")]
public sealed class AdminController : ControllerBase
{
    private readonly ISender _sender;
    public AdminController(ISender sender) => _sender = sender;

    [HttpPost("users/{userId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid userId, CancellationToken ct)
    {
        await _sender.Send(new DeactivateUserCommand(userId), ct);
        return NoContent();
    }

    [HttpPut("reviews/{reviewId}/moderation")]
    public async Task<IActionResult> Moderate(Guid reviewId, [FromBody] ModerationRequest request, CancellationToken ct)
    {
        await _sender.Send(new ModerateReviewCommand(reviewId, request.Decision), ct);
        return NoContent();
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard() => Ok(new { Message = "Admin dashboard skeleton" });

    /// <summary>
    /// GET /api/v1/admin/audit-logs
    /// Retrieves PII-redacted audit logs for platform governance.
    /// </summary>
    [HttpGet("audit-logs")]
    public IActionResult GetAuditLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        // TODO: Inject IApplicationDbContext and query AuditLogs with PII redaction
        return Ok(new 
        { 
            Items = Array.Empty<object>(), 
            Page = page, 
            PageSize = pageSize, 
            TotalCount = 0 
        });
    }
}

public sealed record ModerationRequest(string Decision);
