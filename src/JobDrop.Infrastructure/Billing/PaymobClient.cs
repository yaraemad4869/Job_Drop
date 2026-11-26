using JobDrop.Application.Contracts.Billing;

namespace JobDrop.Infrastructure.Billing;

public sealed class PaymobClient : IPaymobClient
{
    public Task<PaymobCheckoutResponse> CreateCheckoutAsync(PaymobCheckoutRequest request, CancellationToken ct)
    {
        // TODO: Implement Paymob integration
        // Constructor: PaymobCheckoutResponse(string checkoutUrl, string providerOrderId, string providerIntentionId)
        return Task.FromResult(new PaymobCheckoutResponse(
            CheckoutUrl: "skeleton-checkout-url",
            ProviderOrderId: $"order-{Guid.NewGuid()}",
            ProviderIntentionId: $"intention-{Guid.NewGuid()}"
        ));
    }

    public Task<bool> VerifyHmacAsync(string payload, string signature, string secret, CancellationToken ct)
    {
        // TODO: Implement HMAC-SHA512 verification
        return Task.FromResult(true);
    }
}
