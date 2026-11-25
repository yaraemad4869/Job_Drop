using JobDrop.Application.Features.Discovery.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Discovery.Queries;

public sealed record SpatialSearchQuery(SpatialSearchCommand Payload) : IRequest<SpatialSearchResult>;