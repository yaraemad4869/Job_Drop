using MediatR;

namespace JobDrop.Application.Features.Insights.Queries;

public sealed record PersonalInsightsQuery(string Period, DateTime? Date) : IRequest<PersonalInsightsResult>;

public sealed record PersonalInsightsResult(
    int SentCount,
    int LandedCount,
    int OpenedCount,
    int ReviewedCount,
    int ShortlistedCount,
    int InterviewedCount,
    int HiredCount,
    int RefundedCount,
    double CvStrengthScore,
    double ProfileCompletenessPct);