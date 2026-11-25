using JobDrop.Application.Common;
using JobDrop.Application.Contracts.Persistence;
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
using JobDrop.Domain.Entities.Sla;
using JobDrop.Domain.Entities.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DropApplication = JobDrop.Domain.Entities.Drops.Application;

namespace JobDrop.Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Identity
    public DbSet<UserSession> UserSessions { get; set; } = null!;
    public DbSet<AccountDeletionRequest> AccountDeletionRequests { get; set; } = null!;

    // Reference
    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;
    public DbSet<OperatingArea> OperatingAreas { get; set; } = null!;

    // Candidates
    public DbSet<CandidateProfile> CandidateProfiles { get; set; } = null!;
    public DbSet<WorkExperience> WorkExperiences { get; set; } = null!;
    public DbSet<Education> Educations { get; set; } = null!;
    public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;
    public DbSet<CandidateLanguage> CandidateLanguages { get; set; } = null!;
    public DbSet<CandidateDocument> CandidateDocuments { get; set; } = null!;
    public DbSet<StartingPoint> StartingPoints { get; set; } = null!;
    public DbSet<SavedRegion> SavedRegions { get; set; } = null!;
    public DbSet<SavedJob> SavedJobs { get; set; } = null!;
    public DbSet<SavedSearch> SavedSearches { get; set; } = null!;
    public DbSet<TargetCompensation> TargetCompensations { get; set; } = null!;
    public DbSet<WorkPermit> WorkPermits { get; set; } = null!;

    // Companies
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<CompanyOffice> CompanyOffices { get; set; } = null!;
    public DbSet<CompanyMember> CompanyMembers { get; set; } = null!;
    public DbSet<CompanyInvitation> CompanyInvitations { get; set; } = null!;
    public DbSet<CompanyCalendar> CompanyCalendars { get; set; } = null!;
    public DbSet<CompanyHoliday> CompanyHolidays { get; set; } = null!;
    public DbSet<OfficeFieldAudit> OfficeFieldAudits { get; set; } = null!;
    public DbSet<VerificationRequest> VerificationRequests { get; set; } = null!;

    // Jobs
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<JobAssignment> JobAssignments { get; set; } = null!;
    public DbSet<JobRequirement> JobRequirements { get; set; } = null!;
    public DbSet<JobDailyIntake> JobDailyIntakes { get; set; } = null!;

    // Drops
    public DbSet<DropWalletBalance> DropWalletBalances { get; set; } = null!;
    public DbSet<DropLedgerEntry> DropLedgerEntries { get; set; } = null!;
    public DbSet<DropApplication> Applications { get; set; } = null!;
    public DbSet<ApplicationSnapshot> ApplicationSnapshots { get; set; } = null!;
    public DbSet<ApplicationEvent> ApplicationEvents { get; set; } = null!;
    public DbSet<PinnedDrop> PinnedDrops { get; set; } = null!;

    // SLA
    public DbSet<ApplicationSla> ApplicationSlas { get; set; } = null!;

    // Reverse Drops
    public DbSet<ReverseDropPool> ReverseDropPools { get; set; } = null!;
    public DbSet<ReverseDrop> ReverseDrops { get; set; } = null!;

    // Messaging
    public DbSet<Conversation> Conversations { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<InterviewProposal> InterviewProposals { get; set; } = null!;

    // Billing
    public DbSet<Plan> Plans { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<PaymentWebhookEvent> PaymentWebhookEvents { get; set; } = null!;

    // Reviews
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<ReviewReport> ReviewReports { get; set; } = null!;

    // Observability
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<ExportJob> ExportJobs { get; set; } = null!;

    // Compliance
    public DbSet<ConsentAuditLog> ConsentAuditLogs { get; set; } = null!;

    // Storage
    public DbSet<StorageUploadIntent> StorageUploadIntents { get; set; } = null!;

    // IUnitOfWork implementation
    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Explicitly ignore DomainEvent to prevent EF Core from trying to map it as an entity
        builder.Ignore<JobDrop.Domain.Common.DomainEvent>();

        // Normalize Identity table names
        builder.Entity<AppUser>().ToTable("AspNetUsers");
        builder.Entity<AppRole>().ToTable("AspNetRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims");
    }
}