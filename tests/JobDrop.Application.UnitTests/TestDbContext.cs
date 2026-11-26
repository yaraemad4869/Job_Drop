using DropApplication = JobDrop.Domain.Entities.Drops.Application;
using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Common;
using JobDrop.Domain.Entities.Billing;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.Entities.Compliance;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Geography;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Entities.Jobs;
using JobDrop.Domain.Entities.Messaging;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Domain.Entities.Reference;
using JobDrop.Domain.Entities.ReverseDrops;
using JobDrop.Domain.Entities.Storage;
using JobDrop.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.UnitTests;

public sealed class TestDbContext : DbContext, IApplicationDbContext
{
    public TestDbContext(DbContextOptions options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<AppRole> Roles => Set<AppRole>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<AccountDeletionRequest> AccountDeletionRequests => Set<AccountDeletionRequest>();
    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    public DbSet<OperatingArea> OperatingAreas => Set<OperatingArea>();
    public DbSet<CandidateProfile> CandidateProfiles => Set<CandidateProfile>();
    public DbSet<WorkExperience> WorkExperiences => Set<WorkExperience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<CandidateLanguage> CandidateLanguages => Set<CandidateLanguage>();
    public DbSet<CandidateDocument> CandidateDocuments => Set<CandidateDocument>();
    public DbSet<StartingPoint> StartingPoints => Set<StartingPoint>();
    public DbSet<SavedRegion> SavedRegions => Set<SavedRegion>();
    public DbSet<SavedJob> SavedJobs => Set<SavedJob>();
    public DbSet<SavedSearch> SavedSearches => Set<SavedSearch>();
    public DbSet<TargetCompensation> TargetCompensations => Set<TargetCompensation>();
    public DbSet<WorkPermit> WorkPermits => Set<WorkPermit>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyOffice> CompanyOffices => Set<CompanyOffice>();
    public DbSet<CompanyMember> CompanyMembers => Set<CompanyMember>();
    public DbSet<CompanyInvitation> CompanyInvitations => Set<CompanyInvitation>();
    public DbSet<CompanyCalendar> CompanyCalendars => Set<CompanyCalendar>();
    public DbSet<CompanyHoliday> CompanyHolidays => Set<CompanyHoliday>();
    public DbSet<OfficeFieldAudit> OfficeFieldAudits => Set<OfficeFieldAudit>();
    public DbSet<VerificationRequest> VerificationRequests => Set<VerificationRequest>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobAssignment> JobAssignments => Set<JobAssignment>();
    public DbSet<JobRequirement> JobRequirements => Set<JobRequirement>();
    public DbSet<JobDailyIntake> JobDailyIntakes => Set<JobDailyIntake>();
    public DbSet<DropWalletBalance> DropWalletBalances => Set<DropWalletBalance>();
    public DbSet<DropLedgerEntry> DropLedgerEntries => Set<DropLedgerEntry>();
    public DbSet<DropApplication> Applications => Set<DropApplication>();
    public DbSet<ApplicationSnapshot> ApplicationSnapshots => Set<ApplicationSnapshot>();
    public DbSet<ApplicationEvent> ApplicationEvents => Set<ApplicationEvent>();
    public DbSet<PinnedDrop> PinnedDrops => Set<PinnedDrop>();
    public DbSet<ApplicationSla> ApplicationSlas => Set<ApplicationSla>();
    public DbSet<ReverseDropPool> ReverseDropPools => Set<ReverseDropPool>();
    public DbSet<ReverseDrop> ReverseDrops => Set<ReverseDrop>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<InterviewProposal> InterviewProposals => Set<InterviewProposal>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentWebhookEvent> PaymentWebhookEvents => Set<PaymentWebhookEvent>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewReport> ReviewReports => Set<ReviewReport>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<ExportJob> ExportJobs => Set<ExportJob>();
    public DbSet<ConsentAuditLog> ConsentAuditLogs => Set<ConsentAuditLog>();
    public DbSet<StorageUploadIntent> StorageUploadIntents => Set<StorageUploadIntent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Prevent In-Memory DB from throwing errors on keyless domain types
        // that are Value Objects or domain artifacts, not database tables.
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<GeoPoint>();
    }

    public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    // BE-01 to BE-18: New entity sets for testing
    public DbSet<SlaPolicy> SlaPolicies => Set<SlaPolicy>();
    public DbSet<RefundLedgerEntry> RefundLedgerEntries => Set<RefundLedgerEntry>();
    public DbSet<RedactionRecord> RedactionRecords => Set<RedactionRecord>();
    public DbSet<ApplicationActionLog> ApplicationActionLogs => Set<ApplicationActionLog>();
    public DbSet<ConversationLifecycle> ConversationLifecycles => Set<ConversationLifecycle>();
    public DbSet<InterviewProposalState> InterviewProposalStates => Set<InterviewProposalState>();
    public DbSet<DailyIntakeSchedule> DailyIntakeSchedules => Set<DailyIntakeSchedule>();
    public DbSet<ApplicationReviewEvent> ApplicationReviewEvents => Set<ApplicationReviewEvent>();
    public DbSet<MultiModeCommuteResult> MultiModeCommuteResults => Set<MultiModeCommuteResult>();
    public DbSet<CompanyRanking> CompanyRankings => Set<CompanyRanking>();
    public DbSet<CompanyCoverImage> CompanyCoverImages => Set<CompanyCoverImage>();
    public DbSet<LocationPrivacySettings> LocationPrivacySettings => Set<LocationPrivacySettings>();
}

