using JobDrop.Application.Features.Discovery.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public sealed class DiscoveryController : ControllerBase
{
    private readonly ISender _sender;

    public DiscoveryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(SpatialSearchResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SpatialSearch([FromBody] SpatialSearchQuery request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }
}
