using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Billing;
using JobDrop.Application.Contracts.Identity;
using JobDrop.Domain.Entities.Identity;

using JobDrop.Application.Features.Auth.DTOs; 
using JobDrop.Application.Features.Billing.Commands; 

namespace JobDrop.WebApi.Services;

public class MockCurrentUser : ICurrentUser
{
    public Guid? UserId => Guid.NewGuid();
    public string Email => "mock@example.com";
    public string Handle => "mock_handle";
    public IReadOnlyCollection<string> Roles => new List<string> { "Admin", "Candidate" }.AsReadOnly();
    public Guid? CandidateProfileId => Guid.NewGuid();
    public bool IsAuthenticated => true;

    public bool IsInRole(string role) => true;
}

public class MockTokenService : ITokenService
{
    public Task<AuthTokens> GenerateTokensAsync(AppUser user, IEnumerable<string> roles, string? deviceId, CancellationToken cancellationToken)
    {
        // Using positional arguments (no parameter names)
        var tokens = new AuthTokens(
            "mock-access-token",
            "mock-refresh-token",
            DateTimeOffset.UtcNow.AddHours(1)
        );
        return Task.FromResult(tokens);
    }

    public Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return Task.FromResult<Guid?>(Guid.NewGuid());
    }

    public Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MockPaymobClient : IPaymobClient
{
    public Task<PaymobCheckoutResponse> CreateCheckoutAsync(PaymobCheckoutRequest request, CancellationToken cancellationToken)
    {
        // Using positional arguments (no parameter names)
        var response = new PaymobCheckoutResponse(
            "mock-provider-order-id",
            "mock-payment-key",
            "mock-invoice-id"
        );
        return Task.FromResult(response);
    }

    public Task<bool> VerifyHmacAsync(string payload, string signature, string expectedHmacKey, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
