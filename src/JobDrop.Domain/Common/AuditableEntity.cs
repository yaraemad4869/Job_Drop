namespace JobDrop.Domain.Common;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAtUtc { get; set; }
}

public abstract class ConcurrentAuditableEntity : AuditableEntity, IHasRowVersion
{
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}