namespace JobDrop.Domain.Enums;

/// <summary>
/// Application lifecycle states (BE-18).
/// Queued applications do not start SLA countdown until delivered.
/// </summary>
public enum ApplicationStatus
{
    Queued = 0,
    Delivered = 1,
    Opened = 2,
    Shortlisted = 3,
    Interviewed = 4,
    InterviewScheduled = 5,
    Offered = 6,
    OfferExtended = 7,
    Hired = 8,
    Rejected = 9,
    Expired = 10,
    Cancelled = 11,
    Withdrawn = 12
}
