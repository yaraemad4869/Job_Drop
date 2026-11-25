using FluentAssertions;
using JobDrop.Domain.Entities.Identity;
using JobDrop.IntegrationTests.Fixtures;
using Xunit;

namespace JobDrop.IntegrationTests.Smoke;

public class DatabaseTests : IClassFixture<JobDropWebAppFactory>
{
    private readonly JobDropWebAppFactory _factory;

    public DatabaseTests(JobDropWebAppFactory factory) => _factory = factory;

    [Fact]
    public async Task CanInsertAndRetrieveUser()
    {
        using var db = _factory.CreateDbContext();

        var suffix = Guid.NewGuid().ToString("N").Substring(0, 12);
        var user = new AppUser
        {
            UserName = $"smoke_{suffix}@example.com",
            Email = $"smoke_{suffix}@example.com",
            Handle = $"h_{suffix}",
            EmailConfirmed = true
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var retrieved = await db.Users.FindAsync(user.Id);

        retrieved.Should().NotBeNull();
        retrieved!.Email.Should().Be(user.Email);
    }
}