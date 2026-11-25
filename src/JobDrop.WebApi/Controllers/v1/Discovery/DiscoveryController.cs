using JobDrop.Application.Common;
using JobDrop.Application.Features.Discovery.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Discovery;

[ApiController]
[Route("api/v1/discovery")]
public sealed class DiscoveryController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUser _currentUser;

    public DiscoveryController(ISender sender, ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Search for jobs within a specific radius from a given location (Haversine Formula).
    /// </summary>
    [HttpPost("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SpatialSearchResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchJobs([FromBody] SpatialSearchRequest request, CancellationToken cancellationToken)
    {
        var query = new SpatialSearchQuery(
            CandidateProfileId: _currentUser.CandidateProfileId ?? Guid.Empty,
            Latitude: request.Latitude,
            Longitude: request.Longitude,
            RadiusInKm: request.RadiusInKm,
            Page: request.Page,
            PageSize: request.PageSize);

        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}

public sealed record SpatialSearchRequest(
    double Latitude,
    double Longitude,
    double RadiusInKm,
    int Page = 1,
    int PageSize = 20);
