using JobDrop.Application.Features.ReverseDrops.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.ReverseDrops;

[ApiController]
[Route("api/v1/reverse-drops")]
[Authorize]
public sealed class ReverseDropsController : ControllerBase
{
    private readonly ISender _sender;
    public ReverseDropsController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Dispatch([FromBody] DispatchReverseDropRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            return BadRequest("Idempotency-Key header is required.");

        var result = await _sender.Send(new DispatchReverseDropCommand(
            request.CompanyId, request.JobId, request.CandidateProfileId,
            idempotencyKey, request.DispatchNote, request.OfferDocumentId), ct);
        return CreatedAtAction(nameof(Dispatch), new { id = result.ReverseDropId }, result);
    }

    [HttpPost("{id}/respond")]
    public async Task<IActionResult> Respond(Guid id, [FromBody] RespondRequest request, CancellationToken ct)
    {
        await _sender.Send(new RespondToReverseDropCommand(id, request.Accept), ct);
        return NoContent();
    }
}

public sealed record DispatchReverseDropRequest(
    Guid CompanyId, Guid JobId, Guid CandidateProfileId,
    string? DispatchNote, Guid? OfferDocumentId);
public sealed record RespondRequest(bool Accept);