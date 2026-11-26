using MediatR;

namespace JobDrop.Application.Features.Candidates.Queries;

public sealed record GetProfileQuery() : IRequest<object>;