using MediatR;
using JobDrop.Application.DTOs;
using JobDrop.Application.Services;

namespace JobDrop.Application.Features.DossierPreview;

public class GetDossierPreviewHandler : IRequestHandler<GetDossierPreviewQuery, DossierPreviewDto>
{
    private readonly DossierPreviewService _previewService;

    public GetDossierPreviewHandler(DossierPreviewService previewService)
    {
        _previewService = previewService;
    }

    public async Task<DossierPreviewDto> Handle(GetDossierPreviewQuery request, CancellationToken cancellationToken)
    {
        var result = await _previewService.GetPreviewAsync(request.CandidateId, cancellationToken);
        return result ?? throw new InvalidOperationException("Candidate not found");
    }
}
