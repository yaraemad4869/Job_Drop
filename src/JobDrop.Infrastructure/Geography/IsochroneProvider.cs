using JobDrop.Application.Contracts.Geography;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Infrastructure.Geography;

public sealed class IsochroneProvider : IIsochroneProvider
{
    public Task<object> GenerateIsochroneAsync(GeoPoint origin, int budgetSeconds, string mode, CancellationToken ct)
        => Task.FromResult<object>(new { Type = "FeatureCollection", Features = Array.Empty<object>() });
}
