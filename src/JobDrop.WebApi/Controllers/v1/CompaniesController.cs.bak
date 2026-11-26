using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/companies")]
[Authorize(Roles = "CompanyOwner,CompanyMember,PlatformAdmin")]
public sealed class CompaniesController : ControllerBase
{
    // Sprint 2: Endpoints for Company Profile, Offices, Team, JobAssignments
    // Delegated to MediatR handlers in full implementation
    
    [HttpGet("{companyId}/offices")]
    public IActionResult GetOffices(Guid companyId)
    {
        // TODO: Implement GetCompanyOfficesQuery via ISender
        return Ok(new { message = "Sprint 2 Offices endpoint scaffolded" });
    }

    [HttpPost("{companyId}/offices")]
    public IActionResult CreateOffice(Guid companyId, [FromBody] object request)
    {
        // TODO: Implement CreateCompanyOfficeCommand via ISender
        return Created();
    }
}
