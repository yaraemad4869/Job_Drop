using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Billing;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Billing.Handlers;

public sealed class CreateCheckoutCommandHandler
    : IRequestHandler<Commands.CreateCheckoutCommand, Commands.CheckoutResult>
{
    private readonly IApplicationDbContext _db;
    private readonly IPaymobClient _paymob;

    public CreateCheckoutCommandHandler(IApplicationDbContext db, IPaymobClient paymob)
    {
        _db = db;
        _paymob = paymob;
    }

    public async Task<Commands.CheckoutResult> Handle(
        Commands.CreateCheckoutCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _db.Plans
            .FirstOrDefaultAsync(p => p.Code == request.PlanCode, cancellationToken)
            ?? throw new InvalidOperationException($"Plan {request.PlanCode} not found");

        var merchantRef = $"jobdrop-{Guid.NewGuid():N}";

        // Convert decimal PriceAmount to cents (long)
        var amountCents = (long)(plan.PriceAmount * 100);

        var payment = new Payment();
        payment.UserId = Guid.Empty; // TODO: From ICurrentUser
        payment.MerchantReference = merchantRef;
        payment.AmountCents = amountCents;
        payment.Currency = plan.PriceCurrency;
        payment.Status = PaymentStatus.Created;

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(cancellationToken);

        // PaymobCheckoutRequest - using POSITIONAL arguments to avoid name mismatches
        // Signature: (long AmountCents, string Currency, string MerchantOrderId, string ???, string? ???)
        var checkoutRequest = new PaymobCheckoutRequest(
            amountCents,
            plan.PriceCurrency,
            merchantRef,
            "{}",
            null);

        var response = await _paymob.CreateCheckoutAsync(checkoutRequest, cancellationToken);

        payment.SetProviderIds(response.ProviderOrderId, response.ProviderIntentionId);
        await _db.SaveChangesAsync(cancellationToken);

        return new Commands.CheckoutResult(payment.Id, response.CheckoutUrl);
    }
}