using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobDrop.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountDeletionRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    RequestedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    GracePeriodEndsAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDeletionRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Handle = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    AvatarCode = table.Column<int>(type: "integer", nullable: false),
                    GoogleSubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TokenVersion = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    PreferredLanguage = table.Column<int>(type: "integer", nullable: false),
                    PreferredTheme = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    TimeZoneId = table.Column<string>(type: "text", nullable: false),
                    CalendarType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastLoginAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActorType = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: true),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: true),
                    OldValuesJson = table.Column<string>(type: "text", nullable: true),
                    NewValuesJson = table.Column<string>(type: "text", nullable: true),
                    CorrelationId = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    NameAr = table.Column<string>(type: "text", nullable: false),
                    ParentCode = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LegalName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ThemeColorHex = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    CommercialLicenseNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    LogoStorageKey = table.Column<string>(type: "text", nullable: true),
                    Industry = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SizeBand = table.Column<int>(type: "integer", nullable: false),
                    VerificationLevel = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyHolidays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    HolidayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyHolidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AcceptedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    InvitedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInvitations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsentAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConsentType = table.Column<int>(type: "integer", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    ConsentVersion = table.Column<string>(type: "text", nullable: false),
                    ConsentedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DropLedgerEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Bucket = table.Column<int>(type: "integer", nullable: false),
                    Delta = table.Column<int>(type: "integer", nullable: false),
                    ReasonCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RunningBalanceAfter = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropLedgerEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: true),
                    DownloadUrl = table.Column<string>(type: "text", nullable: true),
                    DownloadUrlExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Channel = table.Column<int>(type: "integer", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    DeepLink = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    DeliveredAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeadLetteredAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operating_areas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameAr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TimeZoneId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    boundary_geojson = table.Column<string>(type: "jsonb", nullable: false),
                    center_point = table.Column<string>(type: "jsonb", nullable: false),
                    BoundarySource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsScheduledTransit = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsCar = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsWalking = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsMicromobility = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operating_areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    NextAttemptAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProcessedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeadLetteredAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastError = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    MerchantReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProviderOrderId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProviderIntentionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProviderTransactionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AmountCents = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FailureReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PaidAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    ProviderTransactionId = table.Column<string>(type: "text", nullable: false),
                    EventStateKey = table.Column<string>(type: "text", nullable: false),
                    PayloadHash = table.Column<string>(type: "text", nullable: false),
                    ProcessingStatus = table.Column<string>(type: "text", nullable: false),
                    RawPayload = table.Column<string>(type: "text", nullable: true),
                    ReceivedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProcessedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PinnedDrops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PinOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinnedDrops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Audience = table.Column<string>(type: "text", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceCurrency = table.Column<string>(type: "text", nullable: false),
                    BillingPeriod = table.Column<string>(type: "text", nullable: false),
                    EntitlementsJson = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MarketCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReporterUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    ReportedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    SavedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedSearches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchName = table.Column<string>(type: "text", nullable: false),
                    FiltersJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedSearches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageUploadIntents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetKind = table.Column<int>(type: "integer", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    ExpectedMimeType = table.Column<string>(type: "text", nullable: false),
                    MaxSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageUploadIntents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CurrentPeriodStartUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CurrentPeriodEndUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AutoRenew = table.Column<bool>(type: "boolean", nullable: false),
                    CancelledAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ProviderSubscriptionId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FamilyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceInfo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RevokedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerificationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedLevel = table.Column<int>(type: "integer", nullable: false),
                    EvidenceStorageKey = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ReviewedByAdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewNotes = table.Column<string>(type: "text", nullable: true),
                    RequestedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReviewedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Headline = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    PreferredWorkModesJson = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SharingProtocolJson = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    DropThemeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsOptedInTalentSearch = table.Column<bool>(type: "boolean", nullable: false),
                    ProfileCompletenessPct = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyCalendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkingDaysJson = table.Column<string>(type: "text", nullable: false),
                    WorkStartMinute = table.Column<int>(type: "integer", nullable: false),
                    WorkEndMinute = table.Column<int>(type: "integer", nullable: false),
                    TimeZoneId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCalendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyCalendars_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    DepartmentScope = table.Column<string>(type: "text", nullable: true),
                    JoinedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyMembers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReverseDropPools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CycleStartUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CycleEndUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MonthlyRemaining = table.Column<int>(type: "integer", nullable: false),
                    TopUpRemaining = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReverseDropPools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReverseDropPools_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_offices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperatingAreaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    OfficeType = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "jsonb", nullable: false),
                    GeofenceRadiusMeters = table.Column<int>(type: "integer", nullable: false),
                    TransitAnchorText = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MakaniCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VerificationLevel = table.Column<int>(type: "integer", nullable: false),
                    LocationVersion = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_offices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_offices_operating_areas_OperatingAreaId",
                        column: x => x.OperatingAreaId,
                        principalTable: "operating_areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CandidateDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    OriginalFileName = table.Column<string>(type: "text", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    RedactedStorageKey = table.Column<string>(type: "text", nullable: true),
                    Sha256Hash = table.Column<string>(type: "text", nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    ScanStatus = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    UploadedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateDocuments_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateLanguages_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateSkills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "integer", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedBySource = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateSkills_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DropWalletBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    DailyRemaining = table.Column<int>(type: "integer", nullable: false),
                    DailyLimit = table.Column<int>(type: "integer", nullable: false),
                    DailyResetAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    BonusBalance = table.Column<int>(type: "integer", nullable: false),
                    SpecialBalance = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropWalletBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropWalletBalances_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    DegreeBadge = table.Column<string>(type: "text", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstitutionName = table.Column<string>(type: "text", nullable: false),
                    Faculty = table.Column<string>(type: "text", nullable: true),
                    FieldOfStudy = table.Column<string>(type: "text", nullable: false),
                    GraduationYear = table.Column<int>(type: "integer", nullable: false),
                    EvidenceDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedRegions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperatingAreaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferredModesJson = table.Column<string>(type: "text", nullable: false),
                    ModeMaxSecondsJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedRegions_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "starting_points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AnchorType = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "jsonb", nullable: false),
                    PrivacyZoneLabel = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    IsActiveOrigin = table.Column<bool>(type: "boolean", nullable: false),
                    PlaceReferenceId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_starting_points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_starting_points_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetCompensations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: true),
                    PositionName = table.Column<string>(type: "text", nullable: true),
                    MinAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetCompensations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetCompensations_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    EmployerName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPermits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketCode = table.Column<string>(type: "text", nullable: false),
                    PermitType = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    NeedsSponsorship = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPermits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPermits_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ThemeColorHex = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    Description = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    WorkMode = table.Column<int>(type: "integer", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryMax = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryCurrency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false, defaultValue: "USD"),
                    SalaryPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Monthly"),
                    IsSalaryNegotiable = table.Column<bool>(type: "boolean", nullable: false),
                    DailyIntakeCap = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PublishedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jobs_company_offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "company_offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "office_field_audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyOfficeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ScannedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    gps_point = table.Column<string>(type: "jsonb", nullable: true),
                    FieldEvidenceKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReviewedByAdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_office_field_audits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_office_field_audits_company_offices_CompanyOfficeId",
                        column: x => x.CompanyOfficeId,
                        principalTable: "company_offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BatchRank = table.Column<int>(type: "integer", nullable: false),
                    CoverNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RejectionReasonCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RejectionNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DeliveredAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OpenedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinalizedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_CandidateProfiles_CandidateProfileId",
                        column: x => x.CandidateProfileId,
                        principalTable: "CandidateProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    CanTriage = table.Column<bool>(type: "boolean", nullable: false),
                    CanDispatch = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAssignments_CompanyMembers_CompanyMemberId",
                        column: x => x.CompanyMemberId,
                        principalTable: "CompanyMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobAssignments_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDailyIntakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DeliveredCount = table.Column<int>(type: "integer", nullable: false),
                    CapLimit = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDailyIntakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDailyIntakes_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequirementType = table.Column<string>(type: "text", nullable: false),
                    CatalogItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    ItemName = table.Column<string>(type: "text", nullable: true),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: true),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRequirements_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReverseDrops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferDocumentKey = table.Column<string>(type: "text", nullable: true),
                    DispatchNote = table.Column<string>(type: "text", nullable: true),
                    IsSalaryNegotiable = table.Column<bool>(type: "boolean", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric", nullable: true),
                    SalaryMax = table.Column<decimal>(type: "numeric", nullable: true),
                    SalaryCurrency = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SentAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RespondedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReverseDrops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReverseDrops_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReverseDrops_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<int>(type: "integer", nullable: false),
                    ToStatus = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    ActorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorType = table.Column<int>(type: "integer", nullable: false),
                    OccurredAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationEvents_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSlas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DueAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RespondedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PolicyVersion = table.Column<string>(type: "text", nullable: false),
                    CalendarSnapshotJson = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSlas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationSlas_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResumeVersion = table.Column<int>(type: "integer", nullable: false),
                    RedactedResumeKey = table.Column<string>(type: "text", nullable: true),
                    ProfileSnapshotJson = table.Column<string>(type: "text", nullable: false),
                    MatchBreakdownJson = table.Column<string>(type: "text", nullable: false),
                    MatchScore = table.Column<decimal>(type: "numeric", nullable: false),
                    ConsentVersion = table.Column<string>(type: "text", nullable: false),
                    SharingProtocolJson = table.Column<string>(type: "text", nullable: false),
                    FrozenAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationSnapshots_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReverseDropId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsArchivedByCandidate = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchivedByCompany = table.Column<bool>(type: "boolean", nullable: false),
                    LastMessageAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conversations_ReverseDrops_ReverseDropId",
                        column: x => x.ReverseDropId,
                        principalTable: "ReverseDrops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InterviewProposals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CallType = table.Column<int>(type: "integer", nullable: false),
                    SlotsJson = table.Column<string>(type: "text", nullable: false),
                    ConfirmedStartUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConfirmedEndUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    MeetingUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewProposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewProposals_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    AttachmentKey = table.Column<string>(type: "text", nullable: true),
                    ReadAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEvents_ApplicationId",
                table: "ApplicationEvents",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CandidateProfileId_JobId",
                table: "Applications",
                columns: new[] { "CandidateProfileId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CompanyId",
                table: "Applications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobId_Status_DeliveredAtUtc",
                table: "Applications",
                columns: new[] { "JobId", "Status", "DeliveredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSlas_ApplicationId",
                table: "ApplicationSlas",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSnapshots_ApplicationId",
                table: "ApplicationSnapshots",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UQ_Users_GoogleSub",
                table: "AspNetUsers",
                column: "GoogleSubjectId",
                unique: true,
                filter: "\"GoogleSubjectId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Handle",
                table: "AspNetUsers",
                column: "Handle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDocuments_CandidateProfileId",
                table: "CandidateDocuments",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateLanguages_CandidateProfileId",
                table: "CandidateLanguages",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateProfiles_UserId",
                table: "CandidateProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkills_CandidateProfileId",
                table: "CandidateSkills",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_VerificationLevel",
                table: "Companies",
                column: "VerificationLevel");

            migrationBuilder.CreateIndex(
                name: "ix_company_offices_company_verification",
                table: "company_offices",
                columns: new[] { "CompanyId", "VerificationLevel" });

            migrationBuilder.CreateIndex(
                name: "IX_company_offices_OperatingAreaId",
                table: "company_offices",
                column: "OperatingAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCalendars_CompanyId",
                table: "CompanyCalendars",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMembers_CompanyId",
                table: "CompanyMembers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMembers_UserId",
                table: "CompanyMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ApplicationId",
                table: "Conversations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ReverseDropId",
                table: "Conversations",
                column: "ReverseDropId");

            migrationBuilder.CreateIndex(
                name: "IX_DropLedgerEntries_CandidateProfileId_CreatedAtUtc",
                table: "DropLedgerEntries",
                columns: new[] { "CandidateProfileId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_DropLedgerEntries_IdempotencyKey",
                table: "DropLedgerEntries",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DropWalletBalances_CandidateProfileId",
                table: "DropWalletBalances",
                column: "CandidateProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_CandidateProfileId",
                table: "Educations",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewProposals_ConversationId",
                table: "InterviewProposals",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_CompanyMemberId",
                table: "JobAssignments",
                column: "CompanyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_JobId",
                table: "JobAssignments",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDailyIntakes_JobId_MarketDate",
                table: "JobDailyIntakes",
                columns: new[] { "JobId", "MarketDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobRequirements_JobId",
                table: "JobRequirements",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyId_Status",
                table: "Jobs",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_OfficeId",
                table: "Jobs",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Status_PublishedAtUtc",
                table: "Jobs",
                columns: new[] { "Status", "PublishedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_office_field_audits_CompanyOfficeId",
                table: "office_field_audits",
                column: "CompanyOfficeId");

            migrationBuilder.CreateIndex(
                name: "uq_office_field_audits_hash",
                table: "office_field_audits",
                column: "AuditHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_operating_areas_market_code",
                table: "operating_areas",
                column: "MarketCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MerchantReference",
                table: "Payments",
                column: "MerchantReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ProviderTransactionId",
                table: "Payments",
                column: "ProviderTransactionId",
                unique: true,
                filter: "\"ProviderTransactionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReverseDropPools_CompanyId",
                table: "ReverseDropPools",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReverseDrops_CompanyId",
                table: "ReverseDrops",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReverseDrops_JobId",
                table: "ReverseDrops",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedRegions_CandidateProfileId",
                table: "SavedRegions",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "ix_starting_points_candidate",
                table: "starting_points",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetCompensations_CandidateProfileId",
                table: "TargetCompensations",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_RefreshTokenHash",
                table: "UserSessions",
                column: "RefreshTokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_User_Active",
                table: "UserSessions",
                columns: new[] { "UserId", "RevokedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_CandidateProfileId",
                table: "WorkExperiences",
                column: "CandidateProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPermits_CandidateProfileId",
                table: "WorkPermits",
                column: "CandidateProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDeletionRequests");

            migrationBuilder.DropTable(
                name: "ApplicationEvents");

            migrationBuilder.DropTable(
                name: "ApplicationSlas");

            migrationBuilder.DropTable(
                name: "ApplicationSnapshots");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CandidateDocuments");

            migrationBuilder.DropTable(
                name: "CandidateLanguages");

            migrationBuilder.DropTable(
                name: "CandidateSkills");

            migrationBuilder.DropTable(
                name: "CatalogItems");

            migrationBuilder.DropTable(
                name: "CompanyCalendars");

            migrationBuilder.DropTable(
                name: "CompanyHolidays");

            migrationBuilder.DropTable(
                name: "CompanyInvitations");

            migrationBuilder.DropTable(
                name: "ConsentAuditLogs");

            migrationBuilder.DropTable(
                name: "DropLedgerEntries");

            migrationBuilder.DropTable(
                name: "DropWalletBalances");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "ExportJobs");

            migrationBuilder.DropTable(
                name: "InterviewProposals");

            migrationBuilder.DropTable(
                name: "JobAssignments");

            migrationBuilder.DropTable(
                name: "JobDailyIntakes");

            migrationBuilder.DropTable(
                name: "JobRequirements");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "office_field_audits");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentWebhookEvents");

            migrationBuilder.DropTable(
                name: "PinnedDrops");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "ReverseDropPools");

            migrationBuilder.DropTable(
                name: "ReviewReports");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SavedJobs");

            migrationBuilder.DropTable(
                name: "SavedRegions");

            migrationBuilder.DropTable(
                name: "SavedSearches");

            migrationBuilder.DropTable(
                name: "starting_points");

            migrationBuilder.DropTable(
                name: "StorageUploadIntents");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TargetCompensations");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "VerificationRequests");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "WorkPermits");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CompanyMembers");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "ReverseDrops");

            migrationBuilder.DropTable(
                name: "CandidateProfiles");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "company_offices");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "operating_areas");
        }
    }
}
