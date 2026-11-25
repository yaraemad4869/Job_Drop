using System.Security.Claims;
using JobDrop.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.WebApi.Middleware;

public sealed class TokenRevocationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRevocationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IApplicationDbContext db)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var sub = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? context.User.FindFirstValue("sub");
            var tv = context.User.FindFirstValue("tv");

            if (Guid.TryParse(sub, out var userId) && int.TryParse(tv, out var tokenVersion))
            {
                var userTv = await db.Users.AsNoTracking()
                    .Where(u => u.Id == userId)
                    .Select(u => (int?)u.TokenVersion)
                    .FirstOrDefaultAsync(context.RequestAborted);

                var isActive = await db.Users.AsNoTracking()
                    .Where(u => u.Id == userId)
                    .Select(u => (bool?)u.IsActive)
                    .FirstOrDefaultAsync(context.RequestAborted);

                if (!userTv.HasValue || userTv.Value != tokenVersion || isActive != true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { type = "urn:jobdrop:problem:token-revoked", title = "Token has been revoked.", status = 401 });
                    return;
                }
            }
        }
        await _next(context);
    }
}