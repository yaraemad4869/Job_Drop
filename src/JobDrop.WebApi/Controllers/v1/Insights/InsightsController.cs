using JobDrop.Application.Features.Insights.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Insights;

[ApiController]
[Route("api/v1/insights")]
[Authorize]
public sealed class InsightsController : ControllerBase
{
    private readonly ISender _sender;
    public InsightsController(ISender sender) => _sender = sender;

    /// <summary>
    /// GET /api/v1/insights/personal?period=month&date=2026-11-26
    /// Returns candidate's personal funnel metrics.
    /// </summary>
    [HttpGet("personal")]
    public async Task<IActionResult> GetPersonalInsights(
        [FromQuery] string period = "month",
        [FromQuery] DateTime? date = null,
        CancellationToken ct = default)
    {
        var query = new PersonalInsightsQuery(period, date);
        var result = await _sender.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/insights/market?scopeType=city&scopeId={id}&searchContextId={id}
    /// Returns market-wide aggregations (requires N >= 15 sample size).
    /// </summary>
    [HttpGet("market")]
    public async Task<IActionResult> GetMarketInsights(
        [FromQuery] string scopeType,
        [FromQuery] Guid scopeId,
        [FromQuery] string? searchContextId = null,
        CancellationToken ct = default)
    {
        var query = new MarketInsightsQuery(scopeType, scopeId, searchContextId);
        var result = await _sender.Send(query, ct);
        return Ok(result);
    }
}