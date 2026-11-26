using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.DossierPreview;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class DossierPreviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public DossierPreviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{candidateId}")]
    public async Task<IActionResult> GetPreview(Guid candidateId)
    {
        var query = new GetDossierPreviewQuery(candidateId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
