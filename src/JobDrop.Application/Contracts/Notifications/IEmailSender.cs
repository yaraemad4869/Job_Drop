namespace JobDrop.Application.Contracts.Notifications;

public sealed record EmailMessage(
    string To,
    string Subject,
    string HtmlBody,
    string? TextBody = null,
    IReadOnlyList<string>? Attachments = null);

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}