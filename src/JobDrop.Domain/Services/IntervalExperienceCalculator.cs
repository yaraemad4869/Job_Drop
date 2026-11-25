using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.Services;

public sealed class IntervalExperienceCalculator
{
    public int ComputeNonOverlappingMonths(IEnumerable<WorkExperience> experiences)
    {
        var ranges = experiences
            .Select(e => new DateRange(e.StartDate, e.EndDate))
            .OrderBy(r => r.StartDate)
            .ToList();

        if (ranges.Count == 0) return 0;

        var merged = new List<DateRange>();
        DateOnly currentStart = ranges[0].StartDate;
        DateOnly currentEnd = ranges[0].EndDate ?? DateOnly.FromDateTime(DateTime.Today);

        for (int i = 1; i < ranges.Count; i++)
        {
            var next = ranges[i];
            var nextEnd = next.EndDate ?? DateOnly.FromDateTime(DateTime.Today);

            if (next.StartDate <= currentEnd.AddDays(1))
            {
                if (nextEnd > currentEnd) currentEnd = nextEnd;
            }
            else
            {
                merged.Add(new DateRange(currentStart, currentEnd));
                currentStart = next.StartDate;
                currentEnd = nextEnd;
            }
        }
        merged.Add(new DateRange(currentStart, currentEnd));

        int totalMonths = 0;
        foreach (var r in merged)
        {
            totalMonths += ((r.EndDate!.Value.Year - r.StartDate.Year) * 12)
                         + r.EndDate!.Value.Month - r.StartDate.Month + 1;
        }
        return totalMonths;
    }
}