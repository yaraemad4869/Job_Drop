namespace JobDrop.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateOnly StartDate { get; }
    public DateOnly? EndDate { get; }
    public bool IsCurrent => !EndDate.HasValue;

    public DateRange(DateOnly startDate, DateOnly? endDate)
    {
        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date must be >= start date.");
        StartDate = startDate;
        EndDate = endDate;
    }

    public int ComputeOverlappingMonths(DateRange other)
    {
        var start = StartDate > other.StartDate ? StartDate : other.StartDate;
        var end = EndDate ?? DateOnly.FromDateTime(DateTime.Today);
        var otherEnd = other.EndDate ?? DateOnly.FromDateTime(DateTime.Today);
        var endMin = end < otherEnd ? end : otherEnd;
        if (endMin < start) return 0;
        return ((endMin.Year - start.Year) * 12) + endMin.Month - start.Month + 1;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}