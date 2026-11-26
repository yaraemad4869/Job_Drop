using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JobDrop.WebApi.Middlewares;

public class TokenRevocationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRevocationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // TODO: Integrate with ITokenService to check if the user's TokenVersion 
        // matches the one in the JWT claim. If not, return 401 Unauthorized.
        // For now, it passes through to allow the app to run safely.
        await _next(context);
    }
}
