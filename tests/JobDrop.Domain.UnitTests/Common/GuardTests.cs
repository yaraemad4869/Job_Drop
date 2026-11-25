using Xunit;
using FluentAssertions;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.UnitTests.Common;

public class GuardTests
{
    [Fact]
    public void AgainstNull_NullValue_ShouldThrow()
    {
        string? value = null;
        var act = () => Guard.AgainstNull(value);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AgainstNull_NonNullValue_ShouldReturnValue()
    {
        var value = "test";
        var result = Guard.AgainstNull(value);
        result.Should().Be("test");
    }

    [Fact]
    public void AgainstNullOrEmpty_EmptyString_ShouldThrow()
    {
        var act = () => Guard.AgainstNullOrEmpty("");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AgainstNullOrEmpty_WhitespaceString_ShouldThrow()
    {
        var act = () => Guard.AgainstNullOrEmpty("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AgainstNullOrEmpty_ValidString_ShouldReturnValue()
    {
        var result = Guard.AgainstNullOrEmpty("test");
        result.Should().Be("test");
    }

    [Fact]
    public void AgainstEmpty_EmptyGuid_ShouldThrow()
    {
        var act = () => Guard.AgainstEmpty(Guid.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AgainstEmpty_ValidGuid_ShouldReturnValue()
    {
        var guid = Guid.NewGuid();
        var result = Guard.AgainstEmpty(guid);
        result.Should().Be(guid);
    }

    [Fact]
    public void AgainstNegative_NegativeInt_ShouldThrow()
    {
        var act = () => Guard.AgainstNegative(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AgainstNegative_ZeroInt_ShouldReturnValue()
    {
        var result = Guard.AgainstNegative(0);
        result.Should().Be(0);
    }

    [Fact]
    public void AgainstNegativeOrZero_ZeroInt_ShouldThrow()
    {
        var act = () => Guard.AgainstNegativeOrZero(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void AgainstNegative_NegativeDecimal_ShouldThrow()
    {
        var act = () => Guard.AgainstNegative(-1.5m);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}