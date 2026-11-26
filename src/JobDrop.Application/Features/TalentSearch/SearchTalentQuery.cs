using MediatR;
using JobDrop.Application.DTOs;

namespace JobDrop.Application.Features.TalentSearch;

public record SearchTalentQuery(TalentSearchRequest Request) : IRequest<TalentSearchResponse>;

public record TalentSearchResponse(
    int TotalCount,
    int PageNumber,
    int PageSize,
    List<TalentSearchResult> Results);

public record TalentSearchResult(
    Guid CandidateId,
    string CandidateCode,
    double MatchScore,
    string MatchBand,
    int ExperienceYears,
    List<string> Skills);
