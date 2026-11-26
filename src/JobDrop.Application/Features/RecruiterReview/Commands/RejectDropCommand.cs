using MediatR;

namespace JobDrop.Application.Features.RecruiterReview.Commands;

public sealed record RejectDropCommand(Guid DropId, string ReasonCode, string? Note) : IRequest<bool>;