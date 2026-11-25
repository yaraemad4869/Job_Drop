using JobDrop.Application.Contracts.Notifications;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.Notifications;

public sealed class ConsoleEmailSender : IEmailSender
{
    private readonly ILogger<ConsoleEmailSender> _logger;
    public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger) => _logger = logger;

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[EMAIL] To={To} Subject={Subject}", message.To, message.Subject);
        return Task.CompletedTask;
    }
}