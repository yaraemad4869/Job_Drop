# JobDrop Sprint 4 & 5 Implementation

## Sprint 4: Reverse Drops, Chats, Scheduling, Insights

### Delivered Features

#### 1. Reverse Drops (Talent Hunting)
- **DispatchReverseDropCommandHandler**: Atomic credit consumption from ReverseDropPool
- **RespondToReverseDropCommandHandler**: Accept/Decline with conversation creation
- **48h Exclusivity Window**: Auto-expiry with credit refund
- **Idempotency**: Prevents duplicate dispatches via IdempotencyKey

#### 2. Messaging & Scheduling
- **SendMessageCommandHandler**: Conversation-scoped messaging with participant validation
- **ProposeInterviewCommandHandler**: Voice/Video interview proposals with slot management
- **Conversation Entity**: Links Applications, ReverseDrops, Companies, Candidates

#### 3. Insights
- **PersonalInsightsQueryHandler**: Funnel metrics from immutable ApplicationEvents
  - Sent, Landed, Opened, Reviewed, Shortlisted, Interviewed, Hired, Refunded counts
  - CV Strength Score & Profile Completeness %
- **MarketInsightsQueryHandler**: Market-wide aggregations with minimum sample size (N >= 15)
  - High match density, best nearby match, compensation baseline
  - Ranked roles, sector breakdown, skill gaps

#### 4. Special Drop Gifting
- **TransferSpecialDropCommandHandler**: Peer-to-peer @handle transfers
- **Atomic Ledger**: Immutable DropLedgerEntries for audit trail
- **Feature Flag**: Behind configurable gate

### Domain Services
- **DropWalletPolicy**: Full atomic consumption (Daily -> Bonus -> Special)
- **SlaDeadlineCalculator**: 48h elapsed with calendar/holiday awareness
- **MatchScoringEngine**: Weighted scoring (0.35 semantic + 0.30 commute + 0.20 experience + 0.15 compensation)
- **CandidateCodeGenerator**: Anonymous #JD-XXXX codes for blind screening
- **BlindDossierProjector**: Progressive disclosure (Blind -> Disclosed on Interview/Offer)

---

## Sprint 5: Paymob, Outbox, Background Workers, Production Readiness

### Delivered Features

#### 1. Paymob Billing (Hardened)
- **CreateCheckoutCommandHandler**: Server-side plan resolution, ProviderOrderId binding
- **HandlePaymobWebhookCommandHandler**: 
  - HMAC-SHA512 verification (constant-time)
  - Bind via signed `order.id` (NEVER trust unsigned `merchant_order_id`)
  - Idempotent state transitions (Pending -> Paid -> Refunded)
  - Atomic subscription activation
  - 64KB payload limit enforcement

#### 2. Transactional Outbox
- **OutboxNotificationWorker**: Multi-channel dispatch (InApp, Email, WhatsApp, Push)
- **Exponential Backoff**: 1m, 5m, 15m, 1h, 4h retry schedule
- **Dead-Letter Queue**: After >= 5 failed attempts
- **IEmailSender Integration**: SMTP/API adapter

#### 3. Background Workers (Full Implementation)
- **QueueDispatchWorker**: 
  - Bounded batch (Take(50))
  - Re-validates Job.Status, Company.IsActive, Candidate.IsActive
  - Atomic promotion Queued -> Delivered
  - Refunds invalid drops to BonusBalance
- **PaymobReconcilerWorker**: 
  - Polls ProviderPending payments > 15m old
  - Flags mismatches for Admin review
- **ReverseDropExpiryWorker**: 48h auto-expiry with pool credit refund
- **DocumentScanWorker**: Magic-byte, malware scan, SHA-256, CV redaction
- **AccountDeletionPurgeWorker**: GDPR right-to-be-forgotten orchestration

#### 4. Export Engine
- **CreateExportJobCommandHandler**: Async export (DropsCsv, DropsXlsx, PersonalInsightsXlsx, RequisitionsCsv)
- **202 Accepted**: Returns JobId immediately
- **Background Processing**: Non-blocking API

#### 5. Integration Tests
- **DropWalletConcurrencyTests**: 20 concurrent attempts for last credit -> exactly 1 succeeds
- **PaymobWebhookSecurityTests**: HMAC, order.id binding, idempotency, state transitions
- **ReverseDropLifecycleTests**: Dispatch, Accept, Decline, 48h expiry
- **BlindScreeningTests**: No PII leak, name masking, progressive disclosure

### Configuration
- **Paymob**: BaseUrl, ApiKey, HmacSecret, IntegrationId
- **Storage**: Provider (S3/Azure/R2), BucketName, Region, MaxFileSizeBytes, MaxFilesPerProfile
- **RateLimiting**: SearchSlidingWindow (60/min), DropSlidingWindow (5/min), LoginBucket (5/15min), ExportConcurrencyLimit (1)

---

## Security & Compliance

### Zero-Trust Validation
- FluentValidation on all Commands/Queries
- Resource-level authorization (IDOR prevention)
- JobAssignment scoping for recruiters

### Privacy & GDPR
- Blind screening until Interview/Offer
- 1.2km privacy buffer on StartingPoints
- ConsentAuditLogs for every Drop dispatch
- AccountDeletionPurgeWorker for right-to-be-forgotten

### Observability
- Serilog structured JSON (PII-redacted)
- X-Correlation-Id threading
- RFC 7807 ProblemDetails for all errors
- Outbox pattern for reliable notifications

---

## Next Steps (Sprint 6 - Production Readiness)

1. **Load Testing**: k6/Artillery scripts for concurrent Drop dispatch
2. **Staging Verification**: End-to-end workflow in staging environment
3. **CI/CD Pipeline**: GitHub Actions with SQL Server Testcontainers
4. **Monitoring**: Application Insights / Grafana dashboards
5. **Runbook**: Operational procedures for workers, reconciliation, purge

---

## Definition of Done Checklist

- [x] Clean Architecture: Zero infrastructure in Domain
- [x] Async & Concurrency: CancellationToken threaded, atomic transactions, RowVersion
- [x] EF Core Discipline: AsNoTracking + Select on reads, zero N+1
- [x] Zero-Trust Security: FluentValidation, resource-level checks, blind screening
- [x] Error Contract: RFC 7807 ProblemDetails, OpenAPI updated
- [x] Automated Tests: Unit + Integration covering Happy, Negative, Boundary, IDOR, Concurrency
- [x] Background Workers: DB leases, bounded batches, idempotent keys
- [x] Paymob Hardened: order.id binding, HMAC, idempotent state transitions
- [x] Outbox: Multi-channel, exponential backoff, dead-letter