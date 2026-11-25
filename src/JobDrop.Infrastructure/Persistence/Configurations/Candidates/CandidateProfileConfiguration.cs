using JobDrop.Domain.Entities.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Candidates;

public sealed class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
{
    public void Configure(EntityTypeBuilder<CandidateProfile> builder)
    {
        builder.ToTable("CandidateProfiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Headline).HasMaxLength(200);
        builder.Property(x => x.Summary).HasMaxLength(4000);
        builder.Property(x => x.PreferredWorkModesJson).HasMaxLength(200);
        builder.Property(x => x.SharingProtocolJson).HasMaxLength(4000);
        builder.Property(x => x.DropThemeCode).HasMaxLength(50);
        builder.Property(x => x.ProfileCompletenessPct).HasPrecision(5, 2);
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasOne(x => x.User).WithOne().HasForeignKey<CandidateProfile>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}