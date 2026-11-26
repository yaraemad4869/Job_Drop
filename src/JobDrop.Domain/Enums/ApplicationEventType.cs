namespace JobDrop.Domain.Enums;

/// <summary>
/// Immutable application lifecycle event types for audit trail and insights.
/// </summary>
public enum ApplicationEventType
{
    Sent = 0,
    Landed = 1,
    Queued = 2,
    Delivered = 3,
    Opened = 4,
    Reviewed = 5,
    Commented = 6,
    Shortlisted = 7,
    InterviewScheduled = 8,
    Assessment = 9,
    OfferExtended = 10,
    CounterOffered = 11,
    Hired = 12,
    Rejected = 13,
    Withdrawn = 14,
    Refunded = 15,
    Cancelled = 16,
    Expired = 17
}