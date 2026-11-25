namespace JobDrop.Application.Features.Auth.DTOs;

public sealed record RegisterCandidateRequest(
    string Email,
    string Password,
    string Handle,
    string? FullName);

public sealed record RegisterCompanyRequest(
    string Email,
    string Password,
    string Handle,
    string CompanyLegalName,
    string CompanyDisplayName);

public sealed record LoginRequest(string Email, string Password);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAtUtc,
    string Handle,
    string Email,
    IReadOnlyList<string> Roles);

public sealed record BootstrapResponse(
    Guid UserId,
    string Handle,
    string Email,
    int AvatarCode,
    bool IsVerified,
    string ActivePortalMode,
    IReadOnlyList<CompanyMembershipDto> CompanyMemberships,
    WalletBalancesDto WalletBalances,
    int UnreadChats,
    int UnreadNotifications,
    UserPreferencesDto Preferences,
    IReadOnlyList<string> ConnectedOAuthProviders,
    DateTimeOffset ServerTimeUtc);

public sealed record CompanyMembershipDto(Guid CompanyId, string DisplayName, int Role);

public sealed record WalletBalancesDto(int DailyRemaining, int DailyLimit, int BonusBalance, int SpecialBalance);

public sealed record UserPreferencesDto(
    int Language,
    int Theme,
    string Currency,
    string TimeZoneId,
    int CalendarType);

public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public sealed record ForgotPasswordRequest(string Email);

public sealed record ResetPasswordRequest(string Email, string Token, string NewPassword);

public sealed record VerifyEmailRequest(string Email, string Token);