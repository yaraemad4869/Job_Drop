using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed record AddSkillCommand() : IRequest<object>;