namespace JobDrop.Application.Contracts.Storage;

public interface ICvRedactor
{
    Task<string> RedactAsync(string sourceStorageKey, Guid candidateId, CancellationToken cancellationToken = default);
}