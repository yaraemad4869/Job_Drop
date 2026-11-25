using FluentAssertions;
using JobDrop.IntegrationTests.Fixtures;
using Xunit;

namespace JobDrop.IntegrationTests.Smoke;

public class HostStartupTests : IClassFixture<JobDropWebAppFactory>
{
    private readonly JobDropWebAppFactory _factory;

    public HostStartupTests(JobDropWebAppFactory factory) => _factory = factory;

    [Fact]
    public void WebApplication_ShouldBuildHostAndCreateClient()
    {
        var act = () => _factory.CreateClient();

        act.Should().NotThrow("Program.cs must build the host successfully");
    }
}