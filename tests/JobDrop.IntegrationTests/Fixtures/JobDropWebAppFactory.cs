using JobDrop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobDrop.IntegrationTests.Fixtures;

public class JobDropWebAppFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString = "Host=localhost;Port=5433;Database=jobdrop_test;Username=postgres;Password=admin";
    private bool _isInitialized;
    private readonly object _lock = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_connectionString, npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
            });
        });
    }

    public ApplicationDbContext CreateDbContext()
    {
        var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        lock (_lock)
        {
            if (!_isInitialized)
            {
                // 1. Ensure a completely clean state for every test run
                db.Database.EnsureDeleted();
                
                // 2. Apply all migrations
                db.Database.Migrate();
                
                // 3. Fix PostgreSQL RowVersion limitation: 
                // EF Core expects the DB to generate it, but Postgres doesn't for bytea without triggers.
                // Making it nullable bypasses the NOT NULL constraint error during INSERT.
                try 
                {
                    db.Database.ExecuteSqlRaw(@"ALTER TABLE ""CandidateProfiles"" ALTER COLUMN ""RowVersion"" DROP NOT NULL;");
                }
                catch { } // Ignore if already nullable or doesn't exist
                
                _isInitialized = true;
            }
        }
        
        return db;
    }
}