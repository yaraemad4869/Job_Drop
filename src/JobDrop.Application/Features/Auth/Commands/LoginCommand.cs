using JobDrop.Application.Features.Auth.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed record LoginCommand(string Email, string Password, string? DeviceInfo) : IRequest<AuthResponse>;