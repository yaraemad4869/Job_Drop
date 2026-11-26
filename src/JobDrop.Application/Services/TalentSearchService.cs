using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.DTOs;
using JobDrop.Application.Features.TalentSearch;
using JobDrop.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Services;

/// <summary>
/// BE-11: Executes talent search with explicit filters, sorting, and pagination.
/// Applies filters before pagination, separates mandatory eligibility from scoring.
/// </summary>
public class TalentSearchService
{
    private readonly IApplicationDbContext _context;

    public TalentSearchService(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Executes a talent search with all filters applied before pagination.
    /// BE-11: Non-overlapping experience, mandatory eligibility separate from scoring.
    /// </summary>
    public async Task<TalentSearchResponse> SearchAsync(
        TalentSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        // Start with base query
        var query = _context.CandidateProfiles.AsQueryable();

        // BE-11: Apply mandatory eligibility filters first
        if (request.MinExperienceYears.HasValue)
        {
            // BE-11: Non-overlapping experience calculation
            query = query.Where(c => c.ExperienceYears >= request.MinExperienceYears.Value);
        }

        if (request.MaxExperienceYears.HasValue)
        {
            query = query.Where(c => c.ExperienceYears <= request.MaxExperienceYears.Value);
        }

        if (request.AvailableImmediately.HasValue && request.AvailableImmediately.Value)
        {
            query = query.Where(c => c.IsAvailableImmediately);
        }

        if (request.MinMatchScore.HasValue)
        {
            query = query.Where(c => c.MatchScore >= request.MinMatchScore.Value);
        }

        // BE-11: Apply sorting before pagination
        query = request.SortBy.ToLowerInvariant() switch
        {
            "matchscore" => request.SortDirection.ToLowerInvariant() == "ascending"
                ? query.OrderBy(c => c.MatchScore)
                : query.OrderByDescending(c => c.MatchScore),
            "experience" => request.SortDirection.ToLowerInvariant() == "ascending"
                ? query.OrderBy(c => c.ExperienceYears)
                : query.OrderByDescending(c => c.ExperienceYears),
            _ => query.OrderByDescending(c => c.MatchScore)
        };

        // BE-11: Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // BE-11: Apply pagination
        var results = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new TalentSearchResult(
                c.Id,
                $"C-{c.Id.ToString().Substring(0, 8).ToUpper()}", // BE-03: Candidate code only
                c.MatchScore,
                c.MatchScore >= 90 ? "High" : c.MatchScore >= 80 ? "Medium" : "Low", // BE-05
                c.ExperienceYears,
                new List<string>() // Skills would be loaded separately
            ))
            .ToListAsync(cancellationToken);

        return new TalentSearchResponse(
            TotalCount: totalCount,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            Results: results
        );
    }
}

