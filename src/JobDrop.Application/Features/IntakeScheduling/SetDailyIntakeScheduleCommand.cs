using MediatR;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.Application.Features.IntakeScheduling;

public record SetDailyIntakeScheduleCommand(SetDailyIntakeScheduleRequest Request) : IRequest<IntakeScheduleResult>;

public record IntakeScheduleResult(bool Success, string Message);
