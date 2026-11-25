using JobDrop.Application.Features.Auth.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed record RegisterCandidateCommand(
    string Email,
    string Password,
    string Handle,
    string? FullName) : IRequest<AuthResponse>;