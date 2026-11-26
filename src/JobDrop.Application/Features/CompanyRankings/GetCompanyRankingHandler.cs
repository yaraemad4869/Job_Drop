using MediatR;
using JobDrop.Application.Services;

namespace JobDrop.Application.Features.CompanyRankings;

public class GetCompanyRankingHandler : IRequestHandler<GetCompanyRankingQuery, CompanyRankingDto>
{
    private readonly CompanyRankingService _rankingService;

    public GetCompanyRankingHandler(CompanyRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    public async Task<CompanyRankingDto> Handle(GetCompanyRankingQuery request, CancellationToken cancellationToken)
    {
        // BE-14: Delegate to service with ranking logic
        var result = await _rankingService.GetRankingAsync(request.CompanyId, request.Scope, cancellationToken);
        return result ?? throw new InvalidOperationException("Company not found or not eligible for ranking");
    }
}
