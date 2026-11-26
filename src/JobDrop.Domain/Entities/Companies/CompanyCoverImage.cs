namespace JobDrop.Domain.Entities.Companies;

/// <summary>
/// BE-15: Company cover image with storage target and validation.
/// </summary>
public class CompanyCoverImage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public int WidthPixels { get; set; }
    public int HeightPixels { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTimeOffset UploadedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public Guid UploadedByUserId { get; set; }
}
