namespace JobDrop.Application.Contracts.Notifications;

public sealed record WhatsAppMessage(
    string ToPhoneNumber,
    string TemplateName,
    string Locale,
    IReadOnlyDictionary<string, string> Parameters);

public interface IWhatsAppSender
{
    Task SendAsync(WhatsAppMessage message, CancellationToken cancellationToken = default);
}