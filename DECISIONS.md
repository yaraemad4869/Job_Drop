# JobDrop Backend Alignment - Complete Decision Log

This document tracks all product decisions for BE-01 through BE-18.

## Decision Status Legend
- **APPROVED**: Decision made and implemented
- **PENDING**: Awaiting product approval
- **IMPLEMENTED**: Code exists, pending final validation

---

## BE-01: SLA Duration and Countdown Start
**Status**: IMPLEMENTED
**Approved Policy**: `DueAtUtc = DeliveredAtUtc + 48 elapsed hours`
**Implementation**: `SLADeadlineCalculator.cs`, `SlaPolicy.cs`
**Notes**: Policy version stored per application. Queued applications do not consume response window.

---

## BE-02: Qualifying Response and Refund Safety
**Status**: IMPLEMENTED
**Approved Policy**: `Shortlisted` qualifies; `Opened` and `Downloaded CV` do not.
**Implementation**: `RefundSafetyService.cs`, `RefundLedgerEntry.cs`
**Notes**: At most one refund per debit. Atomic transactions for state + wallet + ledger + outbox.

---

## BE-03: Blind Mode - Name Removal
**Status**: IMPLEMENTED
**Approved Policy**: Complete name removal, no partial names like "Sara A."
**Implementation**: `BlindModeService.cs`, `RedactionRecord.cs`
**Notes**: Redaction workflow required. Never falls back to original on failure.

---

## BE-04: Gender Filter and Avatars
**Status**: APPROVED
**Approved Policy**: Use neutral avatars; no gender filter.
**Implementation**: `NeutralAvatarService.cs`
**Notes**: Ambiguous filter replaced with explicit field or removed.

---

## BE-05: Match Band Thresholds
**Status**: APPROVED
**Approved Policy**: High >= 90, Medium >= 80, Low < 80
**Implementation**: `MatchBand.cs`, `SLADeadlineCalculator.cs`
**Notes**: Consistent across Map, Stream, Details, Insights, Talent Search.

---

## BE-06: Interviews, Offers, Invitations
**Status**: IMPLEMENTED
**Approved Policy**: Distinct commands for each action.
**Implementation**: `ApplicationActionLog.cs`
**Notes**: Each action has separate authorization, state transitions, credit rules.

---

## BE-07: Conversation Lifecycle
**Status**: IMPLEMENTED
**Approved Policy**: Dispatch creates pending invitation; acceptance enables messaging.
**Implementation**: `ConversationLifecycle.cs`, `ConversationState.cs`
**Notes**: 48-hour exclusivity window. Atomic acceptance/decline/expiration.

---

## BE-08: Appointment Proposals
**Status**: IMPLEMENTED
**Approved Policy**: Proposals can exist before conversation acceptance.
**Implementation**: `InterviewProposalState.cs`
**Notes**: Supports Voice/Video, multiple slots, time zones.

---

## BE-09: Attachment Limits
**Status**: APPROVED
**Approved Policy**: 10 MB per Drop (independent from 20 MB profile limit).
**Implementation**: `DropAttachmentValidator.cs`, `DomainConstants.cs`
**Notes**: 5 supporting attachments max. Server-side validation.

---

## BE-10: Daily Intake Scheduling
**Status**: IMPLEMENTED
**Approved Policy**: Effective periods, manual pauses preserved.
**Implementation**: `DailyIntakeSchedule.cs`, `IntakeSchedulerService.cs`
**Notes**: Atomic company and job capacity reservations.

---

## BE-11: Talent Search Contract
**Status**: IMPLEMENTED
**Approved Policy**: Explicit filters, pre-pagination sorting.
**Implementation**: `TalentSearchRequest.cs`
**Notes**: Non-overlapping experience. Mandatory eligibility separate from scoring.

---

## BE-12: Review & Comment Events
**Status**: IMPLEMENTED
**Approved Policy**: Internal notes separate from candidate feedback.
**Implementation**: `ApplicationReviewEvent.cs`
**Notes**: Visibility scope enforced. Reviewing does not satisfy SLA unless approved.

---

## BE-13: Multi-Mode Commute
**Status**: IMPLEMENTED
**Approved Policy**: Job qualifies when ANY selected mode satisfies threshold.
**Implementation**: `MultiModeCommuteResult.cs`, `MultiModeCommuteService.cs`
**Notes**: Per-mode results returned. Consistent policy across screens.

---

## BE-14: Company Rankings
**Status**: IMPLEMENTED
**Approved Policy**: Ordinal rank with eligible count. No misleading positions.
**Implementation**: `CompanyRanking.cs`, `CompanyRankingService.cs`
**Notes**: Follow-up rate with sample size validation. Percentile separate from rank.

---

## BE-15: Company Cover Images
**Status**: IMPLEMENTED
**Approved Policy**: Public cover images separate from private documents.
**Implementation**: `CompanyCoverImage.cs`
**Notes**: Storage validation, ownership, update permissions enforced.

---

## BE-16: Dossier Preview
**Status**: IMPLEMENTED
**Approved Policy**: Preview without creating application or debiting wallet.
**Implementation**: `DossierPreviewDto.cs`, `DossierPreviewService.cs`
**Notes**: No side effects. Represents current profile version.

---

## BE-17: Location Privacy
**Status**: IMPLEMENTED
**Approved Policy**: Separate internal and shared precision.
**Implementation**: `LocationPrivacySettings.cs`, `LocationPrivacyService.cs`
**Notes**: No differential privacy claims without defined mechanism.

---

## BE-18: Queued Applications
**Status**: APPROVED
**Approved Policy**: Queued applications do not start SLA countdown.
**Implementation**: `ApplicationStatus.cs`
**Notes**: Distinct delivered/queued states. No duplicate debits/refunds.

---

## Next Steps

1. All implementations are complete and compilable.
2. Database migrations needed for new entities.
3. Unit tests needed for new services.
4. Integration with frontend team for API contract validation.
5. Performance testing for high-volume scenarios.

