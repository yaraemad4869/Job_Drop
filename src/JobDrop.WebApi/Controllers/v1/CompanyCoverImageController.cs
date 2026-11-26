using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.CompanyCovers;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "CompanyAdmin")]
public class CompanyCoverImageController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyCoverImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{companyId}/upload")]
    public async Task<IActionResult> Upload(Guid companyId, [FromForm] IFormFile file)
    {
        var command = new UploadCompanyCoverCommand(companyId, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

