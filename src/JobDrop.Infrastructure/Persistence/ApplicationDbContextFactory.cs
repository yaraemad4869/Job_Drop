using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace JobDrop.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Read connection string from WebApi appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../JobDrop.WebApi"))
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection") 
            ?? "Host=localhost;Database=JobDrop;Username=postgres;Password=postgres";

        // CRITICAL: UseNetTopologySuite MUST be called here for dotnet ef tools to work
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
