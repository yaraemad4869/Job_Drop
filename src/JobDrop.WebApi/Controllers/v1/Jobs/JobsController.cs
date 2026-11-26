using JobDrop.Application.Features.Jobs.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Jobs;

[ApiController]
[Route("api/v1/jobs")]
[Authorize]
public sealed class JobsController : ControllerBase
{
    private readonly ISender _sender;
    public JobsController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateJobCommand(
            request.CompanyId, request.OfficeId, request.Title, request.Description,
            request.WorkMode, request.SalaryMin, request.SalaryMax, request.SalaryCurrency,
            request.DailyIntakeCap), ct);
        return CreatedAtAction(nameof(Create), new { id = result.JobId }, result);
    }

    [HttpPost("{jobId}/publish")]
    public async Task<IActionResult> Publish(Guid jobId, CancellationToken ct)
    {
        await _sender.Send(new PublishJobCommand(jobId), ct);
        return NoContent();
    }

    [HttpPut("{jobId}/intake-policy")]
    public async Task<IActionResult> UpdateIntake(Guid jobId, [FromBody] IntakePolicyRequest request, CancellationToken ct)
    {
        await _sender.Send(new UpdateIntakePolicyCommand(jobId, request.NewDailyCap), ct);
        return NoContent();
    }
}

public sealed record CreateJobRequest(
    Guid CompanyId, Guid OfficeId, string Title, string Description, string WorkMode,
    int? SalaryMin, int? SalaryMax, string SalaryCurrency, int DailyIntakeCap);
public sealed record IntakePolicyRequest(int NewDailyCap);