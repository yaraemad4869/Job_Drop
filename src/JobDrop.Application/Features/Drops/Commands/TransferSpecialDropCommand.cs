using MediatR;

namespace JobDrop.Application.Features.Drops.Commands;

/// <summary>
/// Gifts Special Drop credit to @handle (peer-to-peer transfer).
/// Behind feature flag & rate limit.
/// </summary>
public sealed record TransferSpecialDropCommand(string RecipientHandle, int Amount) : IRequest<bool>;