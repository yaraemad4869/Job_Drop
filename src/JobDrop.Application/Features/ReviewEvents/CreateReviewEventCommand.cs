using MediatR;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.Application.Features.ReviewEvents;

public record CreateReviewEventCommand(CreateReviewEventRequest Request) : IRequest<ReviewEventResult>;

public record ReviewEventResult(Guid EventId, bool Success);
