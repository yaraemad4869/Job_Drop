using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Reference;

public sealed class CatalogItem : BaseEntity
{
    public string Category { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}