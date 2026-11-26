using JobDrop.Application.Features.Billing.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Billing;

[ApiController]
[Route("api/v1/billing")]
public sealed class BillingController : ControllerBase
{
    private readonly ISender _sender;
    public BillingController(ISender sender) => _sender = sender;

    [Authorize]
    [HttpPost("checkouts")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateCheckoutCommand(request.PlanCode, request.MarketCode), ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("webhooks/paymob")]
    public async Task<IActionResult> PaymobWebhook(CancellationToken ct)
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var signature = Request.Headers["X-HMAC-SHA512"].FirstOrDefault() ?? "";
        await _sender.Send(new HandlePaymobWebhookCommand(body, signature), ct);
        return Ok();
    }
}

public sealed record CheckoutRequest(string PlanCode, string MarketCode);