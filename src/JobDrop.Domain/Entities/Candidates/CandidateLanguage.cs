using System.ComponentModel.DataAnnotations.Schema;
using JobDrop.Domain.Common;
using JobDrop.Domain.Enums;

namespace JobDrop.Domain.Entities.Candidates;

public sealed class CandidateLanguage : BaseEntity
{
    public Guid CandidateProfileId { get; set; }
    public Guid LanguageId { get; set; }
    public LanguageLevel Level { get; set; }

    [NotMapped]
    public bool IsNative => Level == LanguageLevel.Native;
}