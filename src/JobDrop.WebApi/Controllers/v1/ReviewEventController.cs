using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.ReviewEvents;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Recruiter,CompanyAdmin")]
public class ReviewEventController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewEventController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewEventRequest request)
    {
        var command = new CreateReviewEventCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
