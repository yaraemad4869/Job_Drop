using MediatR;

namespace JobDrop.Application.Features.Billing.Commands;

public sealed record HandlePaymobWebhookCommand(string RawPayload, string Signature) : IRequest<bool>;