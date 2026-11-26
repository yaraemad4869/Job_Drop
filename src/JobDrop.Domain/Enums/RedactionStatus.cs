namespace JobDrop.Domain.Enums;

/// <summary>
/// Document redaction workflow states (BE-03).
/// Never fall back to original document when redaction fails.
/// </summary>
public enum RedactionStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3,
    Blocked = 4
}
