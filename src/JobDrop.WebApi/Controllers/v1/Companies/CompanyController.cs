using JobDrop.Application.Features.Companies.Commands;
using JobDrop.Application.Features.Companies.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Companies;

[Authorize(Policy = "Recruiter")]
[Route("api/v1/companies/{companyId}")]
public sealed class CompanyController : ApiControllerBase
{
    [HttpPost("offices")]
    [ProducesResponseType(typeof(CompanyOfficeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOffice(Guid companyId, [FromBody] CreateCompanyOfficeRequest payload, CancellationToken ct)
    {
        var command = new CreateCompanyOfficeCommand(
            companyId, payload.Nickname, payload.OfficeType, payload.Latitude, payload.Longitude, payload.GeofenceRadiusMeters, payload.MakaniCode);
        
        var result = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetOffice), new { companyId, officeId = result.Id }, result);
    }

    [HttpGet("offices/{officeId}")]
    [ProducesResponseType(typeof(CompanyOfficeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOffice(Guid companyId, Guid officeId, CancellationToken ct)
    {
        // TODO: Implement GetOfficeQuery
        return Ok(new CompanyOfficeDto(officeId, "HQ", 0, 0.0, 0.0, 500, 0));
    }
}