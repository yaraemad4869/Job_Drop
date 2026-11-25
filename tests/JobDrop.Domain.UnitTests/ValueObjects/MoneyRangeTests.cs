using Xunit;
using FluentAssertions;
using JobDrop.Domain.ValueObjects;

namespace JobDrop.Domain.UnitTests.ValueObjects;

public class MoneyRangeTests
{
    [Fact]
    public void Constructor_ValidRange_ShouldCreate()
    {
        var range = new MoneyRange(1000, 2000, "USD", "Monthly");
        range.MinAmount.Should().Be(1000);
        range.MaxAmount.Should().Be(2000);
        range.Currency.Should().Be("USD");
        range.Period.Should().Be("Monthly");
    }

    [Fact]
    public void Constructor_MaxLessThanMin_ShouldThrow()
    {
        var act = () => new MoneyRange(2000, 1000, "USD");
        act.Should().Throw<ArgumentException>().WithMessage("*Max must be >= Min*");
    }

    [Fact]
    public void Constructor_NegativeMin_ShouldThrow()
    {
        var act = () => new MoneyRange(-100, 1000, "USD");
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_LowercaseCurrency_ShouldBeUppercase()
    {
        var range = new MoneyRange(1000, 2000, "usd");
        range.Currency.Should().Be("USD");
    }

    [Fact]
    public void Overlaps_OverlappingRanges_ShouldReturnTrue()
    {
        var range1 = new MoneyRange(1000, 2000, "USD");
        var range2 = new MoneyRange(1500, 2500, "USD");
        range1.Overlaps(range2).Should().BeTrue();
    }

    [Fact]
    public void Overlaps_NonOverlappingRanges_ShouldReturnFalse()
    {
        var range1 = new MoneyRange(1000, 2000, "USD");
        var range2 = new MoneyRange(3000, 4000, "USD");
        range1.Overlaps(range2).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_DifferentCurrencies_ShouldReturnFalse()
    {
        var range1 = new MoneyRange(1000, 2000, "USD");
        var range2 = new MoneyRange(1500, 2500, "EUR");
        range1.Overlaps(range2).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_TouchingRanges_ShouldReturnTrue()
    {
        var range1 = new MoneyRange(1000, 2000, "USD");
        var range2 = new MoneyRange(2000, 3000, "USD");
        range1.Overlaps(range2).Should().BeTrue();
    }

    [Fact]
    public void Equals_SameValues_ShouldBeEqual()
    {
        var range1 = new MoneyRange(1000, 2000, "USD", "Monthly");
        var range2 = new MoneyRange(1000, 2000, "USD", "Monthly");
        range1.Should().Be(range2);
    }
}