namespace JobDrop.Domain.Services;

/// <summary>
/// Generates anonymous candidate codes for blind screening.
/// Format: #JD-XXXX (4-digit numeric).
/// </summary>
public sealed class CandidateCodeGenerator
{
    public string Generate(Guid applicationId)
    {
        // Deterministic based on application ID for consistency
        var hash = applicationId.GetHashCode();
        var code = Math.Abs(hash) % 10000;
        return $"#JD-{code:D4}";
    }

    public string GenerateRandom()
    {
        var random = new Random();
        var code = random.Next(0, 10000);
        return $"#JD-{code:D4}";
    }
}