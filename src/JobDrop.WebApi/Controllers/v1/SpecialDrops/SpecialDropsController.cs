using JobDrop.Application.Features.Drops.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.SpecialDrops;

[ApiController]
[Route("api/v1/special-drops")]
[Authorize]
public sealed class SpecialDropsController : ControllerBase
{
    private readonly ISender _sender;
    public SpecialDropsController(ISender sender) => _sender = sender;

    /// <summary>
    /// POST /api/v1/special-drops/transfer
    /// Gifts Special Drop credits to @handle (peer-to-peer).
    /// Behind feature flag & rate limit.
    /// </summary>
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferRequest request,
        CancellationToken ct)
    {
        var command = new TransferSpecialDropCommand(request.RecipientHandle, request.Amount);
        var result = await _sender.Send(command, ct);
        return Ok(new { Success = result });
    }
}

public sealed record TransferRequest(string RecipientHandle, int Amount);