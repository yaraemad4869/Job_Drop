using Xunit;
using FluentAssertions;
using JobDrop.Domain.Common;

namespace JobDrop.Domain.UnitTests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessResult()
    {
        var result = Result.Success();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult()
    {
        var result = Result.Failure("Error message", "ERROR_CODE");
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error message");
        result.ErrorCode.Should().Be("ERROR_CODE");
    }

    [Fact]
    public void SuccessT_ShouldReturnValue()
    {
        var result = Result.Success("test value");
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("test value");
    }

    [Fact]
    public void FailureT_ShouldReturnError()
    {
        var result = Result<string>.Failure("Error", "CODE");
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error");
        result.Value.Should().BeNull();
    }
}