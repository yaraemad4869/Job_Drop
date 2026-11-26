using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobDrop.Infrastructure.Persistence;

/// <summary>
/// Helper for PostgreSQL xmin-based optimistic concurrency.
/// PostgreSQL does not auto-populate bytea columns, so we use the native xmin system column.
/// </summary>
public static class ConcurrencyTokenHelper
{
    /// <summary>
    /// Configures xmin as the concurrency token for an entity.
    /// Must be called for every entity that requires optimistic concurrency.
    /// </summary>
    public static void ConfigureXminConcurrency<T>(EntityTypeBuilder<T> builder) where T : class
    {
        builder.Property<uint>("xmin")
               .HasColumnName("xmin")
               .HasColumnType("xid")
               .ValueGeneratedOnAddOrUpdate()
               .IsConcurrencyToken();
    }
}
