namespace JobDrop.Domain.ValueObjects;

public sealed class TimeWindow : ValueObject
{
    public DateTimeOffset StartUtc { get; }
    public DateTimeOffset EndUtc { get; }

    public TimeWindow(DateTimeOffset startUtc, DateTimeOffset endUtc)
    {
        if (endUtc <= startUtc)
            throw new ArgumentException("End must be after start.");
        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public TimeSpan Duration => EndUtc - StartUtc;
    public bool Contains(DateTimeOffset instant) => instant >= StartUtc && instant <= EndUtc;
    public bool Overlaps(TimeWindow other) => StartUtc < other.EndUtc && EndUtc > other.StartUtc;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartUtc;
        yield return EndUtc;
    }
}