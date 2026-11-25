namespace JobDrop.Domain.ValueObjects;

public sealed class MoneyRange : ValueObject
{
    public decimal MinAmount { get; }
    public decimal MaxAmount { get; }
    public string Currency { get; }
    public string Period { get; }

    public MoneyRange(decimal minAmount, decimal maxAmount, string currency, string period = "Monthly")
    {
        if (minAmount < 0) throw new ArgumentOutOfRangeException(nameof(minAmount));
        if (maxAmount < minAmount) throw new ArgumentException("Max must be >= Min.", nameof(maxAmount));
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency required.", nameof(currency));

        MinAmount = minAmount;
        MaxAmount = maxAmount;
        Currency = currency.ToUpperInvariant();
        Period = period;
    }

    public bool Overlaps(MoneyRange other)
    {
        if (Currency != other.Currency) return false;
        return MinAmount <= other.MaxAmount && MaxAmount >= other.MinAmount;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MinAmount;
        yield return MaxAmount;
        yield return Currency;
        yield return Period;
    }
}