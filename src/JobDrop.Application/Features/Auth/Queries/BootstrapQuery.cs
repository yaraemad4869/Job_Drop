using JobDrop.Application.Features.Auth.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Auth.Queries;

public sealed record BootstrapQuery : IRequest<BootstrapResponse>;