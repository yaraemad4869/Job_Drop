using JobDrop.Domain.Entities.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Candidates;

public sealed class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
{
    public void Configure(EntityTypeBuilder<CandidateProfile> builder)
    {
        builder.ToTable("candidate_profiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Headline).HasMaxLength(200);
        builder.Property(x => x.Summary).HasMaxLength(4000);
        builder.Property(x => x.PreferredWorkModes).HasMaxLength(200);
        builder.Property(x => x.SharingProtocolJson).HasMaxLength(4000);
        builder.Property(x => x.DropThemeCode).HasMaxLength(50);

        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
