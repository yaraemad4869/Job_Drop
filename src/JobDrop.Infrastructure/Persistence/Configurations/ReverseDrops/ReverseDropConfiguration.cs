using JobDrop.Domain.Entities.ReverseDrops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence.Configurations.ReverseDrops;

public sealed class ReverseDropConfiguration : IEntityTypeConfiguration<ReverseDrop>
{
    public void Configure(EntityTypeBuilder<ReverseDrop> builder)
    {
        builder.ToTable("reverse_drops");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasIndex(x => x.CandidateProfileId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ExpiresAtUtc);

        // Optimistic Concurrency
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
