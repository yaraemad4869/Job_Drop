using JobDrop.Domain.Common;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.Entities.Geography;

public sealed class OperatingArea : BaseEntity
{
    public string MarketCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string TimeZoneId { get; set; } = "UTC";

    // GIS data will be stored as JSON strings until external GIS API is integrated
    // Boundary: GeoJSON MultiPolygon as string
    // CenterPoint: GeoPoint serialized as JSON
    public string BoundaryGeoJson { get; set; } = string.Empty;
    public GeoPoint CenterPoint { get; set; } = new GeoPoint(0, 0);

    public string BoundarySource { get; set; } = "Admin";
    public bool IsActive { get; set; } = true;
    public bool SupportsScheduledTransit { get; set; }
    public bool SupportsCar { get; set; } = true;
    public bool SupportsWalking { get; set; } = true;
    public bool SupportsMicromobility { get; set; }
}