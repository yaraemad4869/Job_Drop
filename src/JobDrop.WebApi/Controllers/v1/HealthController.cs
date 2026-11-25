using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1;

[Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { status = "ok", serverTimeUtc = DateTimeOffset.UtcNow });
}