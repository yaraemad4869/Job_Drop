using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.LocationPrivacyPreferences;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class LocationPrivacyController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationPrivacyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateLocationPrivacyRequest request)
    {
        var command = new UpdateLocationPrivacyCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
