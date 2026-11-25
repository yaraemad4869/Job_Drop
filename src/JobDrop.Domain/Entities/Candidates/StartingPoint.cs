using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class StartingPoint : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public string Label { get; set; } = string.Empty;
    public AnchorType AnchorType { get; set; }
    public GeoPoint Location { get; set; } = new GeoPoint(0, 0);
    public string? PrivacyZoneLabel { get; set; }
    public bool IsActiveOrigin { get; set; }
    public string? PlaceReferenceId { get; set; }
}