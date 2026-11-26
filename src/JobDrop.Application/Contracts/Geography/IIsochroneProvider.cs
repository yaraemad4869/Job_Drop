using JobDrop.Domain.ValueObjects;

namespace JobDrop.Application.Contracts.Geography;

public interface IIsochroneProvider
{
    Task<object> GenerateIsochroneAsync(GeoPoint origin, int budgetSeconds, string mode, CancellationToken ct);
}
