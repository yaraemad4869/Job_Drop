using MediatR;
using JobDrop.Application.DTOs;

namespace JobDrop.Application.Features.DossierPreview;

public record GetDossierPreviewQuery(Guid CandidateId) : IRequest<DossierPreviewDto>;
