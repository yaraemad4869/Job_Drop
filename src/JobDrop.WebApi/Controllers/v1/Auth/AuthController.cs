using JobDrop.Application.Features.Auth.Commands;
using JobDrop.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Auth;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register/candidate")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCandidate([FromBody] RegisterCandidateRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCandidateCommand(
            Email: request.Email,
            Password: request.Password,
            Handle: request.Handle,
            FullName: request.FullName,
            Headline: request.Headline);

        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(RegisterCandidate), new { handle = result.Handle }, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password, request.DeviceInfo);
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken, request.AccessToken);
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var command = new LogoutCommand(request.RefreshToken);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
}

public sealed record RegisterCandidateRequest(
    string Email,
    string Password,
    string Handle,
    string FullName,
    string? Headline);

public sealed record LoginRequest(
    string Email,
    string Password,
    string? DeviceInfo);

public sealed record RefreshTokenRequest(
    string RefreshToken,
    string AccessToken);

public sealed record LogoutRequest(
    string RefreshToken);
