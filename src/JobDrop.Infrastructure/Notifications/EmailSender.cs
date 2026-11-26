using JobDrop.Application.Contracts.Notifications;

namespace JobDrop.Infrastructure.Notifications;

public sealed class EmailSender : IEmailSender
{
    public Task SendAsync(EmailMessage message, CancellationToken ct)
    {
        // TODO: Integrate MailKit SMTP
        return Task.CompletedTask;
    }
}
