using MediatR;
using Microsoft.AspNetCore.Http;

namespace JobDrop.Application.Features.CompanyCovers;

public record UploadCompanyCoverCommand(Guid CompanyId, IFormFile File) : IRequest<CompanyCoverResult>;

public record CompanyCoverResult(bool Success, string StorageKey, string Message);

