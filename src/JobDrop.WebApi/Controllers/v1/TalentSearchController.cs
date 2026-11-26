using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.DTOs;
using JobDrop.Application.Features.TalentSearch;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Recruiter,CompanyAdmin")]
public class TalentSearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public TalentSearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] TalentSearchRequest request)
    {
        var query = new SearchTalentQuery(request);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
