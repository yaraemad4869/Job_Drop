using MediatR;

namespace JobDrop.Application.Features.Drops.Queries;

/// <summary>
/// Returns Blind or Disclosed projection based on disclosure stage.
/// </summary>
public sealed record GetDropDossierQuery(Guid DropId) : IRequest<object>;