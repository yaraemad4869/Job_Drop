using JobDrop.Application.Features.Discovery.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Discovery.Queries;

public sealed class SpatialSearchHandler : IRequestHandler<SpatialSearchQuery, SpatialSearchResult>
{
    // TODO: Inject IApplicationDbContext, ICommuteRouter, IOperatingAreaQuery for Sprint 3
    public Task<SpatialSearchResult> Handle(SpatialSearchQuery request, CancellationToken cancellationToken)
    {
        // Sprint 2 Stub: Return mock data to validate API contract
        var mockJobs = new List<JobCardDto>
        {
            new JobCardDto(Guid.NewGuid(), "Senior .NET Engineer", "Tech Corp", 1, 85.5m, 25, false, false),
            new JobCardDto(Guid.NewGuid(), "Backend Developer", "Startup Inc", 0, 72.0m, 45, true, false)
        };

        return Task.FromResult(new SpatialSearchResult(
            SearchContextId: Guid.NewGuid().ToString(),
            Jobs: mockJobs,
            TotalCount: 2,
            NextCursor: null));
    }
}