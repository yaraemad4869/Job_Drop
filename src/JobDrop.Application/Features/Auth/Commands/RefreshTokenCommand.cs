using JobDrop.Application.Features.Auth.DTOs;
using MediatR;

namespace JobDrop.Application.Features.Auth.Commands;

public sealed record RefreshTokenCommand(string RefreshToken, string? DeviceInfo) : IRequest<AuthResponse>;