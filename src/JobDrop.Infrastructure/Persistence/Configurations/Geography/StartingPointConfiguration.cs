using JobDrop.Domain.Entities.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Geography;

public sealed class StartingPointConfiguration : IEntityTypeConfiguration<StartingPoint>
{
    public void Configure(EntityTypeBuilder<StartingPoint> builder)
    {
        builder.ToTable("starting_points");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.Label).IsRequired().HasMaxLength(100);
        builder.Property(x => x.AnchorType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PrivacyZoneLabel).HasMaxLength(100);

        builder.Property(x => x.Latitude).HasPrecision(9, 6).IsRequired();
        builder.Property(x => x.Longitude).HasPrecision(9, 6).IsRequired();

        builder.HasIndex(x => x.CandidateProfileId);
    }
}
