using JobDrop.Application.Features.Messaging.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Messaging;

[ApiController]
[Route("api/v1/conversations")]
[Authorize]
public sealed class MessagingController : ControllerBase
{
    private readonly ISender _sender;
    public MessagingController(ISender sender) => _sender = sender;

    [HttpPost("{conversationId}/messages")]
    public async Task<IActionResult> SendMessage(Guid conversationId, [FromBody] SendMessageRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new SendMessageCommand(conversationId, request.Body, request.AttachmentId), ct);
        return Ok(result);
    }

    [HttpPost("{conversationId}/interview-proposals")]
    public async Task<IActionResult> ProposeInterview(Guid conversationId, [FromBody] ProposeInterviewRequest request, CancellationToken ct)
    {
        var id = await _sender.Send(new ProposeInterviewCommand(conversationId, request.CallType, request.Slots), ct);
        return Ok(new { InterviewProposalId = id });
    }
}

public sealed record SendMessageRequest(string Body, Guid? AttachmentId);
public sealed record ProposeInterviewRequest(string CallType, object[] Slots);