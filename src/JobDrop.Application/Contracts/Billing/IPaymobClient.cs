namespace JobDrop.Application.Contracts.Billing;

public sealed record PaymobCheckoutRequest(
    long AmountCents,
    string Currency,
    string MerchantOrderId,
    string CustomerEmail,
    string? Description);

public sealed record PaymobCheckoutResponse(
    string ProviderOrderId,
    string ProviderIntentionId,
    string CheckoutUrl);

public interface IPaymobClient
{
    Task<PaymobCheckoutResponse> CreateCheckoutAsync(PaymobCheckoutRequest request, CancellationToken cancellationToken = default);
    Task<bool> VerifyHmacAsync(string payload, string signature, string secret, CancellationToken ct);
}
