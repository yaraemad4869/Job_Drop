namespace JobDrop.Domain.Common;

/// <summary>
/// Domain-wide constants for limits and boundaries.
/// Centralized source of truth for BE-01 through BE-18.
/// </summary>
public static class DomainConstants
{
    // BE-09: Attachment limits
    public const long MaxDropAttachmentBytes = 10L * 1024L * 1024L; // 10 MB per Drop
    public const int MaxSupportingAttachmentsPerDrop = 5;
    public const int MaxProfileFiles = 10;
    public const long MaxProfileBytes = 20L * 1024L * 1024L; // 20 MB profile limit

    // BE-01: SLA policy
    public const int DefaultSLAHours = 48;
    public const string DefaultSLAPolicyVersion = "v1";

    // BE-05: Match band thresholds
    public const double HighMatchThreshold = 90.0;
    public const double MediumMatchThreshold = 80.0;
    public const string DefaultScoringVersion = "v1";

    // BE-07: Exclusivity window
    public const int ExclusivityWindowHours = 48;

    // BE-08: Interview proposals
    public const int MaxProposedSlotsPerProposal = 5;

    // BE-11: Talent search
    public const int MaxTargetRoles = 5;
    public const int DefaultTalentSearchPageSize = 20;
    public const int MaxTalentSearchPageSize = 100;

    // BE-14: Company rankings
    public const int MinSampleSizeForRanking = 10;

    // BE-15: Company cover images
    public const long MaxCoverImageBytes = 5L * 1024L * 1024L; // 5 MB
    public const int MinCoverImageWidth = 1200;
    public const int MinCoverImageHeight = 400;

    // BE-17: Location privacy
    public const int DefaultSharedLocationPrecisionMeters = 500;
    public const int InternalLocationPrecisionMeters = 1;

    // General limits
    public const int MaxPinnedDrops = 3;
    public const int MaxSavedRegions = 10;
    public const int MaxStartingPoints = 5;
}
