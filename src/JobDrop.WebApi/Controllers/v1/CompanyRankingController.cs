using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.CompanyRankings;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CompanyRankingController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyRankingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetRanking(Guid companyId, [FromQuery] string scope = "City")
    {
        var query = new GetCompanyRankingQuery(companyId, scope);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

