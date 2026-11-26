using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using JobDrop.Application.Queries;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Infrastructure.Persistence;

namespace JobDrop.Application.UnitTests.Queries;

public class GetAuditLogsQueryTests
{
    [Fact]
    public async Task Handle_WithValidRequest_ReturnsPagedResponse()
    {
        // Arrange - Use In-Memory Database instead of complex mocking
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        // Seed test data
        var actorId1 = Guid.NewGuid();
        var actorId2 = Guid.NewGuid();
        
        context.AuditLogs.AddRange(
            new AuditLog
            {
                ActorUserId = actorId1,
                Action = "Create",
                TargetType = "Job",
                CreatedAtUtc = DateTimeOffset.UtcNow.AddMinutes(-5),
                IpAddress = "127.0.0.1"
            },
            new AuditLog
            {
                ActorUserId = actorId2,
                Action = "Update",
                TargetType = "Job",
                CreatedAtUtc = DateTimeOffset.UtcNow,
                IpAddress = "127.0.0.2"
            }
        );
        await context.SaveChangesAsync();

        var handler = new GetAuditLogsQueryHandler(context);
        var query = new GetAuditLogsQuery(null, null, null, 1, 10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task Handle_WithFilters_ReturnsFilteredResults()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var actorId = Guid.NewGuid();
        context.AuditLogs.AddRange(
            new AuditLog { ActorUserId = actorId, Action = "Create", TargetType = "Job", CreatedAtUtc = DateTimeOffset.UtcNow },
            new AuditLog { ActorUserId = actorId, Action = "Update", TargetType = "Job", CreatedAtUtc = DateTimeOffset.UtcNow },
            new AuditLog { ActorUserId = actorId, Action = "Delete", TargetType = "User", CreatedAtUtc = DateTimeOffset.UtcNow }
        );
        await context.SaveChangesAsync();

        var handler = new GetAuditLogsQueryHandler(context);
        var query = new GetAuditLogsQuery(actorId, "Create", null, 1, 10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalItems);
        Assert.Single(result.Data);
        Assert.Equal("Create", result.Data[0].Action);
    }
}
