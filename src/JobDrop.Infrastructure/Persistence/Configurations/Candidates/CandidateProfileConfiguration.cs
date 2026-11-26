using JobDrop.Domain.Entities.Candidates;
using JobDrop.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.Candidates;

public class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
{
    public void Configure(EntityTypeBuilder<CandidateProfile> builder)
    {
        builder.ToTable("candidate_profiles");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Email)
               .HasMaxLength(256)
               .IsRequired();
        
        builder.HasIndex(e => e.Email)
               .IsUnique()
               .HasDatabaseName("UX_CandidateProfile_Email");
        
        builder.Property(e => e.MatchScore)
               .HasColumnType("numeric(5,2)");
        
        // PostgreSQL xmin-based optimistic concurrency
        ConcurrencyTokenHelper.ConfigureXminConcurrency(builder);
    }
}
