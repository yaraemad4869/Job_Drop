using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDrop.Application.Features.ConversationFlow;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ConversationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{conversationId}/accept")]
    public async Task<IActionResult> Accept(Guid conversationId)
    {
        var command = new AcceptConversationCommand(conversationId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{conversationId}/decline")]
    public async Task<IActionResult> Decline(Guid conversationId)
    {
        var command = new DeclineConversationCommand(conversationId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

