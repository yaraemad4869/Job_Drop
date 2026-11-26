using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.IntakeScheduling;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "CompanyAdmin")]
public class IntakeScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntakeScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SetSchedule([FromBody] SetDailyIntakeScheduleRequest request)
    {
        var command = new SetDailyIntakeScheduleCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
