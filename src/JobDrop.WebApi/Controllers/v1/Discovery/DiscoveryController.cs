using JobDrop.Application.Features.Discovery.DTOs;
using JobDrop.Application.Features.Discovery.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Discovery;

[Authorize(Policy = "JobSeeker")]
[Route("api/v1/discovery")]
public sealed class DiscoveryController : ApiControllerBase
{
    [HttpPost("search")]
    [ProducesResponseType(typeof(SpatialSearchResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] SpatialSearchCommand payload, CancellationToken ct)
    {
        var result = await Mediator.Send(new SpatialSearchQuery(payload), ct);
        return Ok(result);
    }

    [HttpGet("searches/{searchId}/jobs")]
    [ProducesResponseType(typeof(List<JobCardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSearchJobs(string searchId, string? cursor, CancellationToken ct)
    {
        // TODO: Implement GetSearchJobsQuery with cursor pagination
        return Ok(new List<JobCardDto>());
    }
}