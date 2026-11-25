using JobDrop.Application.Contracts.Notifications;
using Microsoft.Extensions.Logging;

namespace JobDrop.Infrastructure.Notifications;

public sealed class ConsoleWhatsAppSender : IWhatsAppSender
{
    private readonly ILogger<ConsoleWhatsAppSender> _logger;
    public ConsoleWhatsAppSender(ILogger<ConsoleWhatsAppSender> logger) => _logger = logger;

    public Task SendAsync(WhatsAppMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[WHATSAPP] To={To} Template={Template}", message.ToPhoneNumber, message.TemplateName);
        return Task.CompletedTask;
    }
}