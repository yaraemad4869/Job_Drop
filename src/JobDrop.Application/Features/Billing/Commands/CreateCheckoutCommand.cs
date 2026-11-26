using MediatR;

namespace JobDrop.Application.Features.Billing.Commands;

public sealed record CreateCheckoutCommand(string PlanCode, string MarketCode) : IRequest<CheckoutResult>;

public sealed record CheckoutResult(Guid PaymentId, string CheckoutUrl);