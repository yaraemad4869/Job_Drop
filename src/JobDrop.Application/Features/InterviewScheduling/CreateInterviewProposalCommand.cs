using MediatR;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.Application.Features.InterviewScheduling;

public record CreateInterviewProposalCommand(CreateInterviewProposalRequest Request) : IRequest<InterviewProposalResult>;

public record InterviewProposalResult(Guid ProposalId, string Status);
