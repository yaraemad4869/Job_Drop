using JobDrop.Application.Contracts.Identity;
using JobDrop.Application.Common;
using MediatR;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed record LogoutCommand(string RefreshToken) : IRequest;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ITokenService _tokens;

    public LogoutCommandHandler(ITokenService tokens) => _tokens = tokens;

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _tokens.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
    }
}

public sealed record LogoutAllCommand : IRequest;

public sealed class LogoutAllCommandHandler : IRequestHandler<LogoutAllCommand>
{
    private readonly ITokenService _tokens;
    private readonly ICurrentUser _currentUser;

    public LogoutAllCommandHandler(ITokenService tokens, ICurrentUser currentUser)
    {
        _tokens = tokens;
        _currentUser = currentUser;
    }

    public async Task Handle(LogoutAllCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is { } userId)
        {
            await _tokens.RevokeAllUserSessionsAsync(userId, cancellationToken);
        }
    }
}