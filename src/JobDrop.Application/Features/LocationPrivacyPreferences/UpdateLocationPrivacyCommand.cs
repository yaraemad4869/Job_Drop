using MediatR;
using JobDrop.Application.DTOs.Requests;

namespace JobDrop.Application.Features.LocationPrivacyPreferences;

public record UpdateLocationPrivacyCommand(UpdateLocationPrivacyRequest Request) : IRequest<LocationPrivacyResult>;

public record LocationPrivacyResult(bool Success, string Message);
