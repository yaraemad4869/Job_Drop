using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.CommuteCalculation;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CommuteController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommuteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] CalculateCommuteRequest request)
    {
        var query = new CalculateMultiModeCommuteQuery(request);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
