using Microsoft.EntityFrameworkCore;
using JobDrop.Domain.Entities.Billing;
using JobDrop.Domain.Entities.Candidates;
using JobDrop.Domain.Entities.Companies;
using JobDrop.Domain.Entities.Compliance;
using JobDrop.Domain.Entities.Geography;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Entities.Jobs;
using JobDrop.Domain.Entities.Messaging;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Domain.Entities.Reference;
using JobDrop.Domain.Entities.ReverseDrops;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Storage;
using DropApplication = JobDrop.Domain.Entities.Drops.Application;
using DropWalletBalance = JobDrop.Domain.Entities.Drops.DropWalletBalance;
using DropLedgerEntry = JobDrop.Domain.Entities.Drops.DropLedgerEntry;
using ApplicationSnapshot = JobDrop.Domain.Entities.Drops.ApplicationSnapshot;
using ApplicationEvent = JobDrop.Domain.Entities.Drops.ApplicationEvent;
using PinnedDrop = JobDrop.Domain.Entities.Drops.PinnedDrop;

namespace JobDrop.Application.Contracts.Persistence;

public interface IApplicationDbContext
{
    // Identity
    DbSet<AppUser> Users { get; }
    DbSet<AppRole> Roles { get; }
    DbSet<UserSession> UserSessions { get; }
    DbSet<AccountDeletionRequest> AccountDeletionRequests { get; }

    // Reference
    DbSet<CatalogItem> CatalogItems { get; }
    DbSet<OperatingArea> OperatingAreas { get; }

    // Candidates
    DbSet<CandidateProfile> CandidateProfiles { get; }
    DbSet<WorkExperience> WorkExperiences { get; }
    DbSet<Education> Educations { get; }
    DbSet<CandidateSkill> CandidateSkills { get; }
    DbSet<CandidateLanguage> CandidateLanguages { get; }
    DbSet<CandidateDocument> CandidateDocuments { get; }
    DbSet<StartingPoint> StartingPoints { get; }
    DbSet<SavedRegion> SavedRegions { get; }
    DbSet<SavedJob> SavedJobs { get; }
    DbSet<SavedSearch> SavedSearches { get; }
    DbSet<TargetCompensation> TargetCompensations { get; }
    DbSet<WorkPermit> WorkPermits { get; }

    // Companies
    DbSet<Company> Companies { get; }
    DbSet<CompanyOffice> CompanyOffices { get; }
    DbSet<CompanyMember> CompanyMembers { get; }
    DbSet<CompanyInvitation> CompanyInvitations { get; }
    DbSet<CompanyCalendar> CompanyCalendars { get; }
    DbSet<CompanyHoliday> CompanyHolidays { get; }
    DbSet<OfficeFieldAudit> OfficeFieldAudits { get; }
    DbSet<VerificationRequest> VerificationRequests { get; }

    // Jobs
    DbSet<Job> Jobs { get; }
    DbSet<JobAssignment> JobAssignments { get; }
    DbSet<JobRequirement> JobRequirements { get; }
    DbSet<JobDailyIntake> JobDailyIntakes { get; }

    // Drops
    DbSet<DropWalletBalance> DropWalletBalances { get; }
    DbSet<DropLedgerEntry> DropLedgerEntries { get; }
    DbSet<DropApplication> Applications { get; }
    DbSet<ApplicationSnapshot> ApplicationSnapshots { get; }
    DbSet<ApplicationEvent> ApplicationEvents { get; }
    DbSet<PinnedDrop> PinnedDrops { get; }

    // SLA
    DbSet<ApplicationSla> ApplicationSlas { get; }

    // Reverse Drops
    DbSet<ReverseDropPool> ReverseDropPools { get; }
    DbSet<ReverseDrop> ReverseDrops { get; }

    // Messaging
    DbSet<Conversation> Conversations { get; }
    DbSet<Message> Messages { get; }
    DbSet<InterviewProposal> InterviewProposals { get; }

    // Billing
    DbSet<Plan> Plans { get; }
    DbSet<Subscription> Subscriptions { get; }
    DbSet<Payment> Payments { get; }
    DbSet<PaymentWebhookEvent> PaymentWebhookEvents { get; }

    // Reviews
    DbSet<Review> Reviews { get; }
    DbSet<ReviewReport> ReviewReports { get; }

    // Observability
    DbSet<OutboxMessage> OutboxMessages { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<ExportJob> ExportJobs { get; }

    // Compliance
    DbSet<ConsentAuditLog> ConsentAuditLogs { get; }

    // Storage
    DbSet<StorageUploadIntent> StorageUploadIntents { get; }

    // Mutations
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}