using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Companies;

public sealed class CompanyOffice : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Guid? OperatingAreaId { get; private set; }
    public string Nickname { get; private set; } = string.Empty;
    public string OfficeType { get; private set; } = string.Empty;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int GeofenceRadiusMeters { get; private set; } = 500;
    public string? TransitAnchorText { get; private set; }
    public int VerificationLevel { get; private set; } = 0;
    public int LocationVersion { get; private set; } = 1;

    private CompanyOffice() { }

    public CompanyOffice(Guid companyId, string nickname, string officeType, double latitude, double longitude, int geofenceRadiusMeters)
    {
        CompanyId = companyId;
        Nickname = nickname;
        OfficeType = officeType;
        SetLocation(latitude, longitude);
        GeofenceRadiusMeters = geofenceRadiusMeters;
        LocationVersion = 1;
    }

    public void SetLocation(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90) throw new ArgumentOutOfRangeException(nameof(latitude));
        if (longitude < -180 || longitude > 180) throw new ArgumentOutOfRangeException(nameof(longitude));
        Latitude = latitude;
        Longitude = longitude;
        LocationVersion++;
        VerificationLevel = 0;
    }
}
