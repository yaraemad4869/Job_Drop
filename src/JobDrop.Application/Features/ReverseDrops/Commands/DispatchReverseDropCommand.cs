using MediatR;

namespace JobDrop.Application.Features.ReverseDrops.Commands;

public sealed record DispatchReverseDropCommand(
    Guid CompanyId,
    Guid JobId,
    Guid CandidateProfileId,
    string IdempotencyKey,
    string? DispatchNote,
    Guid? OfferDocumentId
) : IRequest<DispatchReverseDropResult>;

public sealed record DispatchReverseDropResult(Guid ReverseDropId, DateTime ExpiresAtUtc);