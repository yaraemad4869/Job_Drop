using JobDrop.Application.Features.Auth.Commands;
using JobDrop.Application.Features.Auth.DTOs;
using JobDrop.Application.Features.Auth.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Auth;

[Route("api/v1/auth")]
public sealed class AuthController : ApiControllerBase
{
    [HttpPost("register/candidate")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterCandidate([FromBody] RegisterCandidateRequest request, CancellationToken ct)
    {
        var response = await Mediator.Send(new RegisterCandidateCommand(request.Email, request.Password, request.Handle, request.FullName), ct);
        return CreatedAtAction(nameof(Login), new { }, response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var response = await Mediator.Send(new LoginCommand(request.Email, request.Password, Request.Headers.UserAgent.ToString()), ct);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var response = await Mediator.Send(new RefreshTokenCommand(request.RefreshToken, Request.Headers.UserAgent.ToString()), ct);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        await Mediator.Send(new LogoutCommand(request.RefreshToken), ct);
        return NoContent();
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll(CancellationToken ct)
    {
        await Mediator.Send(new LogoutAllCommand(), ct);
        return NoContent();
    }
}

[Route("api/v1/me")]
public sealed class MeController : ApiControllerBase
{
    [Authorize]
    [HttpGet("bootstrap")]
    [ProducesResponseType(typeof(BootstrapResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Bootstrap(CancellationToken ct)
    {
        var response = await Mediator.Send(new BootstrapQuery(), ct);
        return Ok(response);
    }
}