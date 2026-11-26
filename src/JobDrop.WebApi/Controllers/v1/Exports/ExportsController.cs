using JobDrop.Application.Features.Exports.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Exports;

[ApiController]
[Route("api/v1/exports")]
[Authorize]
public sealed class ExportsController : ControllerBase
{
    private readonly ISender _sender;
    public ExportsController(ISender sender) => _sender = sender;

    /// <summary>
    /// POST /api/v1/exports/jobs
    /// Creates async export job. Returns 202 Accepted + JobId.
    /// Kind: DropsCsv | DropsXlsx | PersonalInsightsXlsx | RequisitionsCsv
    /// </summary>
    [HttpPost("jobs")]
    public async Task<IActionResult> CreateJob(
        [FromBody] CreateExportJobRequest request,
        CancellationToken ct)
    {
        var command = new CreateExportJobCommand(request.Kind);
        var result = await _sender.Send(command, ct);
        return Accepted(result);
    }
}

public sealed record CreateExportJobRequest(string Kind);