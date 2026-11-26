using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class CandidateProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string? Headline { get; private set; }
    public string? Summary { get; private set; }
    public string? PreferredWorkModes { get; private set; }
    public string? SharingProtocolJson { get; private set; }
    public string? DropThemeCode { get; private set; }
    

    private CandidateProfile() { }

    public CandidateProfile(Guid userId, string fullName, string? headline = null)
    {
        UserId = userId;
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Headline = headline;
    }

    public void UpdateProfile(string fullName, string? headline)
    {
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Headline = headline;
    }

    public void UpdateSummary(string? summary)
    {
        Summary = summary;
    }

    public void UpdatePreferredWorkModes(string? preferredWorkModes)
    {
        PreferredWorkModes = preferredWorkModes;
    }

    public void UpdateSharingProtocol(string? sharingProtocolJson)
    {
        SharingProtocolJson = sharingProtocolJson;
    }

    public void UpdateDropTheme(string? dropThemeCode)
    {
        DropThemeCode = dropThemeCode;
    }
    public byte[] RowVersion { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateHeadline(string? headline)
    {
        if (!string.IsNullOrWhiteSpace(headline))
            Headline = headline;
    }


    /// <summary>
    /// Test factory method - creates instance with all properties settable for testing.
    /// In production, use the regular constructor.
    /// </summary>
    public static CandidateProfile CreateForTest(
        Guid userId,
        string fullName,
        Guid? id = null,
        string? headline = null,
        string? summary = null,
        byte[]? rowVersion = null)
    {
        var profile = new CandidateProfile(userId, fullName);
        if (id.HasValue) profile.Id = id.Value;
        if (!string.IsNullOrWhiteSpace(headline)) profile.Headline = headline;
        if (!string.IsNullOrWhiteSpace(summary)) profile.Summary = summary;
        if (rowVersion != null) profile.RowVersion = rowVersion;
        return profile;
    }

    // BE-11: Search and matching fields
    public int ExperienceYears { get; set; }
    public bool IsAvailableImmediately { get; set; }
    public double MatchScore { get; set; }
    public string? MatchBand { get; set; }
}


