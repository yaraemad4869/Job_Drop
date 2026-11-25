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
}