using JobDrop.Application.Features.Candidates.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed record UpdateCandidateProfileCommand(UpdateCandidateProfileCommand.Data Payload) : IRequest<CandidateProfileDto>
{
    public sealed record Data(string? Headline, string? Summary, bool? IsOptedInTalentSearch, string? DropThemeCode);
}