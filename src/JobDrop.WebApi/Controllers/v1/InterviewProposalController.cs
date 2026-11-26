using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.InterviewScheduling;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InterviewProposalController : ControllerBase
{
    private readonly IMediator _mediator;

    public InterviewProposalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInterviewProposalRequest request)
    {
        var command = new CreateInterviewProposalCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
