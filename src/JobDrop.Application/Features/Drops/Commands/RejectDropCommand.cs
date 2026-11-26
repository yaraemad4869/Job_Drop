using MediatR;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed record RejectDropCommand(
    Guid DropId,
    string RejectionReasonCode,
    string? Feedback
) : IRequest<bool>;