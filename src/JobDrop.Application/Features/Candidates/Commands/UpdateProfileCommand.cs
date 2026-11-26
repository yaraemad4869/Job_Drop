using MediatR;

namespace JobDrop.Application.Features.Candidates.Commands;

public sealed record UpdateProfileCommand() : IRequest<object>;