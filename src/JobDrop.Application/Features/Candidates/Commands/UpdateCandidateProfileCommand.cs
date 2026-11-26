using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

/// <summary>
/// Command to update candidate profile fields.
/// Only non-null fields are applied (partial update).
/// </summary>
public sealed record UpdateCandidateProfileCommand(
    Guid ProfileId,
    string? Headline,
    string? Summary
) : IRequest<bool>;