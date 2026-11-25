namespace JobDrop.Domain.Common;

public interface IAuditableEntity
{
    string CreatedBy { get; set; }
    DateTimeOffset CreatedAtUtc { get; set; }
    string? LastModifiedBy { get; set; }
    DateTimeOffset? LastModifiedAtUtc { get; set; }
}

public interface IHasRowVersion
{
    byte[] RowVersion { get; set; }
}