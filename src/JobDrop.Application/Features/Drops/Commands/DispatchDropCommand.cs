using MediatR;
using JobDrop.Domain.Enums;

namespace JobDrop.Application.Features.Drops.Commands;

public sealed record DispatchDropCommand(
    Guid JobId,
    Guid CandidateProfileId,
    string IdempotencyKey) : IRequest<DispatchDropResult>;

public sealed record DispatchDropResult(
    Guid ApplicationId,
    WalletBucket ConsumedFrom,
    DateTime DueAtUtc,
    int BatchRank);
