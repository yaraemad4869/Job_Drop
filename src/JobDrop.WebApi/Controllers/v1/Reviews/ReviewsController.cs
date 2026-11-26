using JobDrop.Application.Features.Reviews.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Reviews;

[ApiController]
[Route("api/v1/reviews")]
[Authorize]
public sealed class ReviewsController : ControllerBase
{
    private readonly ISender _sender;
    public ReviewsController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitReviewRequest request, CancellationToken ct)
    {
        var id = await _sender.Send(new SubmitReviewCommand(
            request.ApplicationId, request.TargetType, request.TargetId, request.Rating, request.Comment), ct);
        return Ok(new { ReviewId = id });
    }
}

public sealed record SubmitReviewRequest(Guid ApplicationId, string TargetType, Guid TargetId, int Rating, string? Comment);