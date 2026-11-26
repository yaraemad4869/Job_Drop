using JobDrop.Application.Features.Companies.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Companies;

[ApiController]
[Route("api/v1/companies")]
[Authorize]
public sealed class CompaniesController : ControllerBase
{
    private readonly ISender _sender;
    public CompaniesController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateCompanyCommand(
            request.LegalName, request.DisplayName, request.Industry, request.Website), ct);
        return CreatedAtAction(nameof(Create), new { id = result.CompanyId }, result);
    }

    [HttpPost("{companyId}/offices")]
    public async Task<IActionResult> AddOffice(Guid companyId, [FromBody] AddOfficeRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new AddOfficeCommand(companyId, request.Nickname, request.OfficeType, request.OperatingAreaId), ct);
        return Ok(result);
    }

    [HttpPost("{companyId}/invitations")]
    public async Task<IActionResult> Invite(Guid companyId, [FromBody] InviteMemberRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new InviteMemberCommand(companyId, request.Email, request.Role), ct);
        return Ok(result);
    }
}

public sealed record CreateCompanyRequest(string LegalName, string DisplayName, string? Industry, string? Website);
public sealed record AddOfficeRequest(string Nickname, string OfficeType, Guid OperatingAreaId);
public sealed record InviteMemberRequest(string Email, string Role);