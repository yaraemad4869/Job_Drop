namespace JobDrop.Application.Contracts.Storage;

public sealed record PresignedUploadIntent(
    string StorageKey,
    string UploadUrl,
    IDictionary<string, string> RequiredHeaders,
    DateTimeOffset ExpiresAtUtc);

public interface IPrivateDocumentStorage
{
    Task<PresignedUploadIntent> CreateUploadIntentAsync(
        string storageKey,
        string contentType,
        long maxSizeBytes,
        TimeSpan validity,
        CancellationToken cancellationToken = default);

    Task<bool> ObjectExistsAsync(string storageKey, CancellationToken cancellationToken = default);

    Task<string> CreateDownloadUrlAsync(
        string storageKey,
        string? contentType,
        TimeSpan validity,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
}