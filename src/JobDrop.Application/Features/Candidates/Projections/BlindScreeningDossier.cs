using JobDrop.Domain.Entities.Candidates;

namespace JobDrop.Application.Features.Candidates.Projections;

public sealed record BlindScreeningDossierDto(
    Guid CandidateProfileId,
    string AnonymizedId,
    string? Summary,
    List<string> Skills,
    List<string> Languages,
    List<WorkExperienceDto> WorkExperiences,
    List<EducationDto> Educations);

public sealed record WorkExperienceDto(string Title, string Company, int DurationMonths);
public sealed record EducationDto(string Degree, string Institution);

public static class CandidateProfileBlindExtensions
{
    public static BlindScreeningDossierDto ToBlindDossier(this CandidateProfile profile)
    {
        return new BlindScreeningDossierDto(
            CandidateProfileId: profile.Id,
            AnonymizedId: $"Candidate-{profile.Id.ToString().Substring(0, 4).ToUpper()}",
            Summary: profile.Summary,
            Skills: new List<string>(),
            Languages: new List<string>(),
            WorkExperiences: new List<WorkExperienceDto>(),
            Educations: new List<EducationDto>()
        );
    }
}
