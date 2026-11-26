using MediatR;
using JobDrop.Application.DTOs;
using JobDrop.Application.Services;

namespace JobDrop.Application.Features.TalentSearch;

public class SearchTalentHandler : IRequestHandler<SearchTalentQuery, TalentSearchResponse>
{
    private readonly TalentSearchService _searchService;

    public SearchTalentHandler(TalentSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<TalentSearchResponse> Handle(SearchTalentQuery request, CancellationToken cancellationToken)
    {
        // BE-11: Delegate to service with full query logic
        return await _searchService.SearchAsync(request.Request, cancellationToken);
    }
}
