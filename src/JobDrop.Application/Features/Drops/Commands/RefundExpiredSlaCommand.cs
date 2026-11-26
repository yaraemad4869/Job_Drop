using MediatR;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed record RefundExpiredSlaCommand(Guid ApplicationId, string IdempotencyKey) : IRequest<bool>;
