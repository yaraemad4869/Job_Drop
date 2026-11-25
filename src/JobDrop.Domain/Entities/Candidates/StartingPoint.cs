using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class StartingPoint : BaseEntity
{
    public Guid CandidateProfileId { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string AnchorType { get; private set; } = string.Empty;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string PrivacyZoneLabel { get; private set; } = string.Empty;
    public bool IsActiveOrigin { get; private set; }

    private StartingPoint() { }

    public StartingPoint(Guid candidateProfileId, string label, string anchorType, double latitude, double longitude)
    {
        CandidateProfileId = candidateProfileId;
        Label = label;
        AnchorType = anchorType;
        SetLocation(latitude, longitude);
        IsActiveOrigin = true;
        PrivacyZoneLabel = "Quantized_1.2km";
    }

    public void SetLocation(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90) throw new ArgumentOutOfRangeException(nameof(latitude));
        if (longitude < -180 || longitude > 180) throw new ArgumentOutOfRangeException(nameof(longitude));
        if (latitude == 0.0 && longitude == 0.0) throw new ArgumentException("Coordinates cannot be (0,0).");
        
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Deactivate() => IsActiveOrigin = false;
    public void Activate() => IsActiveOrigin = true;
}
