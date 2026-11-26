-- BE-01 to BE-18: Database Migration Reference
-- This file documents the required schema changes for the Backend Alignment Addendum

-- BE-01: SLA Policies
CREATE TABLE IF NOT EXISTS "SlaPolicies" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "Version" VARCHAR(50) NOT NULL DEFAULT 'v1',
    "DurationHours" INTEGER NOT NULL DEFAULT 48,
    "UseWorkingHours" BOOLEAN NOT NULL DEFAULT false,
    "CustomRulesJson" TEXT,
    "CreatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- BE-02: Refund Ledger Entries
CREATE TABLE IF NOT EXISTS "RefundLedgerEntries" (
    "Id" UUID PRIMARY KEY,
    "OriginalDebitId" UUID NOT NULL,
    "WalletId" UUID NOT NULL,
    "Amount" DECIMAL(18,2) NOT NULL,
    "Reason" VARCHAR(500) NOT NULL,
    "TriggerEvent" VARCHAR(100) NOT NULL,
    "CreatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "IsProcessed" BOOLEAN NOT NULL DEFAULT false,
    CONSTRAINT "UQ_RefundLedgerEntries_OriginalDebitId" UNIQUE ("OriginalDebitId")
);

-- BE-03: Redaction Records
CREATE TABLE IF NOT EXISTS "RedactionRecords" (
    "Id" UUID PRIMARY KEY,
    "DocumentId" UUID NOT NULL,
    "Status" INTEGER NOT NULL DEFAULT 0,
    "FailureReason" TEXT,
    "RedactedDocumentId" UUID,
    "CompletedAtUtc" TIMESTAMPTZ,
    "ProcessorVersion" VARCHAR(100) NOT NULL DEFAULT ''
);

-- BE-06: Application Action Logs
CREATE TABLE IF NOT EXISTS "ApplicationActionLogs" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "ActionType" VARCHAR(100) NOT NULL,
    "PerformedByUserId" UUID NOT NULL,
    "SourceState" VARCHAR(100),
    "TargetState" VARCHAR(100),
    "ConsumesCredit" BOOLEAN NOT NULL DEFAULT false,
    "CreditAmount" DECIMAL(18,2),
    "PerformedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "MetadataJson" TEXT
);

-- BE-07: Conversation Lifecycles
CREATE TABLE IF NOT EXISTS "ConversationLifecycles" (
    "Id" UUID PRIMARY KEY,
    "ConversationId" UUID NOT NULL,
    "State" INTEGER NOT NULL DEFAULT 0,
    "InitiatorUserId" UUID NOT NULL,
    "RespondentUserId" UUID,
    "CreatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "AcceptedAtUtc" TIMESTAMPTZ,
    "ExpiresAtUtc" TIMESTAMPTZ,
    "IsExclusivityActive" BOOLEAN NOT NULL DEFAULT false,
    "ExclusivityExpiresAtUtc" TIMESTAMPTZ
);

-- BE-08: Interview Proposal States
CREATE TABLE IF NOT EXISTS "InterviewProposalStates" (
    "Id" UUID PRIMARY KEY,
    "ProposalId" UUID NOT NULL,
    "ProposalType" VARCHAR(50) NOT NULL,
    "ProposedSlotsJson" TEXT NOT NULL,
    "TimeZone" VARCHAR(100) NOT NULL DEFAULT 'UTC',
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Pending',
    "ConfirmedSlotId" UUID,
    "CreatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "RespondedAtUtc" TIMESTAMPTZ
);

-- BE-10: Daily Intake Schedules
CREATE TABLE IF NOT EXISTS "DailyIntakeSchedules" (
    "Id" UUID PRIMARY KEY,
    "JobId" UUID NOT NULL,
    "EffectiveFrom" DATE NOT NULL,
    "EffectiveTo" DATE,
    "CapLimit" INTEGER NOT NULL,
    "TimeZone" VARCHAR(100) NOT NULL DEFAULT 'UTC',
    "IsManuallyPaused" BOOLEAN NOT NULL DEFAULT false,
    "CreatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedByUserId" UUID NOT NULL
);

-- BE-12: Application Review Events
CREATE TABLE IF NOT EXISTS "ApplicationReviewEvents" (
    "Id" UUID PRIMARY KEY,
    "ApplicationId" UUID NOT NULL,
    "EventType" VARCHAR(100) NOT NULL,
    "AuthorUserId" UUID NOT NULL,
    "VisibilityScope" VARCHAR(50) NOT NULL DEFAULT 'Internal',
    "Content" TEXT,
    "CreatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "SatisfiesSLA" BOOLEAN NOT NULL DEFAULT false
);

-- BE-13: Multi-Mode Commute Results
CREATE TABLE IF NOT EXISTS "MultiModeCommuteResults" (
    "Id" UUID PRIMARY KEY,
    "OriginPointId" UUID NOT NULL,
    "DestinationPointId" UUID NOT NULL,
    "Mode" INTEGER NOT NULL,
    "DurationMinutes" INTEGER NOT NULL,
    "DistanceKm" DECIMAL(10,2) NOT NULL,
    "SatisfiesThreshold" BOOLEAN NOT NULL,
    "ThresholdMinutes" INTEGER NOT NULL,
    "CalculatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "RoutingProviderResponse" TEXT
);

-- BE-14: Company Rankings
CREATE TABLE IF NOT EXISTS "CompanyRankings" (
    "Id" UUID PRIMARY KEY,
    "CompanyId" UUID NOT NULL,
    "Scope" VARCHAR(50) NOT NULL,
    "ScopeKey" VARCHAR(200) NOT NULL,
    "OrdinalRank" INTEGER NOT NULL,
    "EligibleCompanyCount" INTEGER NOT NULL,
    "Percentile" DECIMAL(5,2),
    "FollowUpRate" DECIMAL(5,4) NOT NULL,
    "SampleSize" INTEGER NOT NULL,
    "CalculatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- BE-15: Company Cover Images
CREATE TABLE IF NOT EXISTS "CompanyCoverImages" (
    "Id" UUID PRIMARY KEY,
    "CompanyId" UUID NOT NULL,
    "StorageKey" VARCHAR(500) NOT NULL,
    "ContentType" VARCHAR(100) NOT NULL,
    "SizeBytes" BIGINT NOT NULL,
    "WidthPixels" INTEGER NOT NULL,
    "HeightPixels" INTEGER NOT NULL,
    "IsPublic" BOOLEAN NOT NULL DEFAULT true,
    "UploadedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "UploadedByUserId" UUID NOT NULL
);

-- BE-17: Location Privacy Settings
CREATE TABLE IF NOT EXISTS "LocationPrivacySettings" (
    "Id" UUID PRIMARY KEY,
    "UserId" UUID NOT NULL,
    "InternalPrecisionMeters" INTEGER NOT NULL DEFAULT 1,
    "SharedPrecisionMeters" INTEGER NOT NULL DEFAULT 500,
    "AllowResidentialPoint" BOOLEAN NOT NULL DEFAULT false,
    "AllowExactLocation" BOOLEAN NOT NULL DEFAULT false,
    "RetentionPurpose" VARCHAR(200) NOT NULL DEFAULT 'Matching',
    "UpdatedAtUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Indexes for performance
CREATE INDEX IF NOT EXISTS "IX_SlaPolicies_ApplicationId" ON "SlaPolicies" ("ApplicationId");
CREATE INDEX IF NOT EXISTS "IX_RefundLedgerEntries_OriginalDebitId" ON "RefundLedgerEntries" ("OriginalDebitId");
CREATE INDEX IF NOT EXISTS "IX_RedactionRecords_DocumentId" ON "RedactionRecords" ("DocumentId");
CREATE INDEX IF NOT EXISTS "IX_ApplicationActionLogs_ApplicationId" ON "ApplicationActionLogs" ("ApplicationId");
CREATE INDEX IF NOT EXISTS "IX_ConversationLifecycles_ConversationId" ON "ConversationLifecycles" ("ConversationId");
CREATE INDEX IF NOT EXISTS "IX_DailyIntakeSchedules_JobId" ON "DailyIntakeSchedules" ("JobId");
CREATE INDEX IF NOT EXISTS "IX_ApplicationReviewEvents_ApplicationId" ON "ApplicationReviewEvents" ("ApplicationId");
CREATE INDEX IF NOT EXISTS "IX_CompanyRankings_CompanyId_Scope" ON "CompanyRankings" ("CompanyId", "Scope");
CREATE INDEX IF NOT EXISTS "IX_CompanyCoverImages_CompanyId" ON "CompanyCoverImages" ("CompanyId");
CREATE INDEX IF NOT EXISTS "IX_LocationPrivacySettings_UserId" ON "LocationPrivacySettings" ("UserId");
