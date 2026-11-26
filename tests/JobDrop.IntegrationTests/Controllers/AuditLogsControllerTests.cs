using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using JobDrop.Application.Queries;
using JobDrop.WebApi.Controllers.v1;

namespace JobDrop.IntegrationTests.Controllers;

public class AuditLogsControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly AuditLogsController _controller;

    public AuditLogsControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new AuditLogsController(_mockMediator.Object);
    }

    [Fact]
    public async Task GetAuditLogs_ValidRequest_ReturnsOkResult()
    {
        var expectedResponse = new AuditLogResponse(0, 1, 10, 0, new List<AuditLogDto>());
        _mockMediator.Setup(m => m.Send(It.IsAny<GetAuditLogsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(expectedResponse);

        var result = await _controller.GetAuditLogs(null, null, null, 1, 10);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, okResult.Value);
    }

    [Fact]
    public async Task GetAuditLogs_WithFilters_PassesCorrectQueryToMediator()
    {
        var actorUserId = Guid.NewGuid();
        var expectedResponse = new AuditLogResponse(1, 1, 10, 1, new List<AuditLogDto>());
        
        GetAuditLogsQuery? capturedQuery = null; // FIXED: Added '?' to allow null
        _mockMediator.Setup(m => m.Send(It.IsAny<GetAuditLogsQuery>(), It.IsAny<CancellationToken>()))
                     .Callback<IRequest<AuditLogResponse>, CancellationToken>((q, ct) => capturedQuery = (GetAuditLogsQuery)q)
                     .ReturnsAsync(expectedResponse);

        await _controller.GetAuditLogs(actorUserId, "Create", "Job", 2, 15);

        Assert.NotNull(capturedQuery);
        Assert.Equal(actorUserId, capturedQuery.ActorUserId);
        Assert.Equal("Create", capturedQuery.Action);
        Assert.Equal("Job", capturedQuery.TargetType);
        Assert.Equal(2, capturedQuery.PageNumber);
        Assert.Equal(15, capturedQuery.PageSize);
    }
}
