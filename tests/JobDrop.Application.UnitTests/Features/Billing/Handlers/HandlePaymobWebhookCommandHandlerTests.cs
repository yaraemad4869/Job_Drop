using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.Billing.Commands;
using JobDrop.Application.Features.Billing.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace JobDrop.Application.UnitTests.Features.Billing.Handlers;

public sealed class HandlePaymobWebhookCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockDb;
    private readonly Mock<IPaymobClient> _mockPaymob;
    private readonly HandlePaymobWebhookCommandHandler _handler;

    public HandlePaymobWebhookCommandHandlerTests()
    {
        _mockDb = new Mock<IApplicationDbContext>();
        _mockPaymob = new Mock<IPaymobClient>();
        
        var options = Options.Create(new PaymobOptions { HmacSecret = "test_secret" });
        var logger = new Mock<ILogger<HandlePaymobWebhookCommandHandler>>();
        
        _handler = new HandlePaymobWebhookCommandHandler(_mockDb.Object, _mockPaymob.Object, options, logger.Object);
    }

    [Fact]
    public async Task Handle_WithInvalidHmac_ShouldReturnFalse()
    {
        // Arrange
        var payload = "{\"order\":{\"id\":\"123\"},\"success\":true,\"transaction\":{\"id\":\"tx_1\"}}";
        var invalidSignature = "invalid_signature";
        var command = new HandlePaymobWebhookCommand(payload, invalidSignature);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}