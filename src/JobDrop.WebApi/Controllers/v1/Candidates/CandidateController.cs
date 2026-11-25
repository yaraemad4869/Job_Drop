using JobDrop.Application.Features.Candidates.Commands;
using JobDrop.Application.Features.Candidates.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Candidates;

[Authorize]
[Route("api/v1/me")]
public sealed class CandidateController : ApiControllerBase
{
    [HttpGet("profile")]
    [ProducesResponseType(typeof(CandidateProfileDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        // TODO: Implement GetProfileQuery
        return Ok(new CandidateProfileDto(Guid.Empty, "", "", 0, false, "classic"));
    }

    [HttpPatch("profile")]
    [ProducesResponseType(typeof(CandidateProfileDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateCandidateProfileRequest payload, CancellationToken ct)
    {
        var command = new UpdateCandidateProfileCommand(new UpdateCandidateProfileCommand.Data(
            payload.Headline, payload.Summary, payload.IsOptedInTalentSearch, payload.DropThemeCode));
        var result = await Mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("starting-points")]
    [ProducesResponseType(typeof(StartingPointDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStartingPoint([FromBody] CreateStartingPointRequest payload, CancellationToken ct)
    {
        var command = new CreateStartingPointCommand(payload.Label, payload.AnchorType, payload.Latitude, payload.Longitude, payload.IsActiveOrigin);
        var result = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetProfile), new { }, result);
    }
}