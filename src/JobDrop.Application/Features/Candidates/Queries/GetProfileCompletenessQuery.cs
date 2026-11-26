using MediatR;

namespace JobDrop.Application.Features.Candidates.Queries;

public sealed record GetProfileCompletenessQuery() : IRequest<object>;