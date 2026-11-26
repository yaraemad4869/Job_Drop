using JobDrop.Domain.Enums;
using JobDrop.Domain.Services;
using Xunit;

namespace JobDrop.IntegrationTests.Features;

/// <summary>
/// Tests blind screening enforcement:
/// - No PII leaked before Interview/Offer
/// - CandidateCode format #JD-XXXX
/// - Name masking "Sara Ahmed" -> "Sara A."
/// - Progressive disclosure on status change
/// </summary>
public sealed class BlindScreeningTests
{
    [Fact]
    public void BlindStage_NoPiiExposed()
    {
        // Arrange
        var projector = new BlindDossierProjector(new CandidateCodeGenerator());

        // Act
        var stage = projector.DetermineStage(ApplicationStatus.Delivered);

        // Assert
        Assert.Equal(DisclosureStage.Blind, stage);
    }

    [Fact]
    public void InterviewStage_FullDisclosure()
    {
        // Arrange
        var projector = new BlindDossierProjector(new CandidateCodeGenerator());

        // Act
        var stage = projector.DetermineStage(ApplicationStatus.InterviewScheduled);

        // Assert
        Assert.Equal(DisclosureStage.Disclosed, stage);
    }

    [Fact]
    public void MaskName_HidesLastName()
    {
        // Arrange
        var projector = new BlindDossierProjector(new CandidateCodeGenerator());

        // Act
        var masked = projector.MaskName("Sara Ahmed");

        // Assert
        Assert.Equal("Sara A.", masked);
    }

    [Fact]
    public void GenerateCandidateCode_FormatCorrect()
    {
        // Arrange
        var generator = new CandidateCodeGenerator();
        var appId = Guid.NewGuid();

        // Act
        var code = generator.Generate(appId);

        // Assert
        Assert.Matches(@"^#JD-\d{4}$", code);
    }

    [Fact]
    public void PrivacyBuffer_QuantizesCoordinates()
    {
        // Arrange
        var projector = new BlindDossierProjector(new CandidateCodeGenerator());
        var lat = 30.0444;
        var lon = 31.2357;

        // Act
        var approx = projector.GetApproximateLocation(lat, lon);

        // Assert
        Assert.Contains("Area", approx);
        Assert.DoesNotContain("30.0444", approx); // Exact coords hidden
    }
}