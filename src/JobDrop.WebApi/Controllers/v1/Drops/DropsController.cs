using JobDrop.Application.Common;
using JobDrop.Application.Features.Drops.Commands;
using JobDrop.Application.Features.Drops.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Drops;

[ApiController]
[Route("api/v1")]
[Authorize]
public sealed class DropsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUser _currentUser;

    public DropsController(ISender sender, ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Dispatch a drop (apply) to a specific job. Requires Idempotency-Key header to prevent duplicate submissions.
    /// </summary>
    [HttpPost("jobs/{jobId:guid}/drops")]
    [ProducesResponseType(typeof(DispatchDropResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DispatchDrop(
        [FromRoute] Guid jobId,
        [FromBody] DispatchDropRequest request,
        CancellationToken cancellationToken)
    {
        // Extract Idempotency-Key from header
        if (!Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKeyValues) ||
            string.IsNullOrWhiteSpace(idempotencyKeyValues.FirstOrDefault()))
        {
            return BadRequest(new { error = "Idempotency-Key header is required.", code = "MISSING_IDEMPOTENCY_KEY" });
        }

        var idempotencyKey = idempotencyKeyValues.FirstOrDefault()!;

        var command = new DispatchDropCommand(
            JobId: jobId,
            CandidateProfileId: _currentUser.CandidateProfileId ?? throw new UnauthorizedAccessException("Candidate profile not found."),
            IdempotencyKey: idempotencyKey);

        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetMyDrops), new { }, result);
    }

    /// <summary>
    /// Get all drops (applications) for the current candidate with SLA status.
    /// </summary>
    [HttpGet("me/drops")]
    [ProducesResponseType(typeof(List<MyDropDto>), StatusCodes.Status200OK)]

    /// Returns Blind or Disclosed dossier based on disclosure stage.
    /// </summary>
    [HttpGet("{dropId:guid}/dossier")]
    public async Task<IActionResult> GetDossier(Guid dropId, CancellationToken ct)
    {
        var query = new JobDrop.Application.Features.Drops.Queries.GetDropDossierQuery(dropId);
        var result = await _sender.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/drops/my-drops
    /// Returns candidate's own drops.
    /// CandidateProfileId is extracted from the authenticated user.
    /// </summary>
    [HttpGet("my-drops")]
    public async Task<IActionResult> GetMyDrops(CancellationToken ct)
    {
        var candidateProfileId = _currentUser.CandidateProfileId 
            ?? throw new UnauthorizedAccessException("Candidate profile not found for current user");
        
        var query = new JobDrop.Application.Features.Drops.Queries.GetMyDropsQuery(candidateProfileId);
        var result = await _sender.Send(query, ct);
        return Ok(result);
    }
}

public sealed record DispatchDropRequest(string IdempotencyKey);
