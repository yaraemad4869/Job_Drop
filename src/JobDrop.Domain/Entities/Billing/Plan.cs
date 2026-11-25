using JobDrop.Domain.Common;

namespace JobDrop.Domain.Entities.Billing;

public sealed class Plan : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "USD";
    public string BillingPeriod { get; set; } = "Monthly";
    public string EntitlementsJson { get; set; } = "{}";
    public int Version { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public string? MarketCode { get; set; }
}