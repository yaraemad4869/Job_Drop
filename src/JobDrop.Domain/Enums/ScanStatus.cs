namespace JobDrop.Domain.Enums;

public enum ScanStatus
{
    PendingScan = 0,
    Scanning = 1,
    Clean = 2,
    Quarantined = 3,
    Failed = 4
}