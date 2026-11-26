using MediatR;

namespace JobDrop.Application.Features.CompanyRankings;

public record GetCompanyRankingQuery(Guid CompanyId, string Scope) : IRequest<CompanyRankingDto>;

public record CompanyRankingDto(
    Guid CompanyId,
    int OrdinalRank,
    int EligibleCompanyCount,
    double? Percentile,
    double FollowUpRate);

