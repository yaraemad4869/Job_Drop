using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobDrop.Infrastructure.Persistence;

public sealed class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("JOBDROP_CONNECTIONSTRING")
            ?? "Host=localhost;Port=5433;Database=jobdrop_dev;Username=postgres;Password=admin";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            })
            .Options;

        return new ApplicationDbContext(options);
    }
}