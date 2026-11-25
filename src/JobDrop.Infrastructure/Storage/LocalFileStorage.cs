using JobDrop.Application.Contracts.Storage;

namespace JobDrop.Infrastructure.Storage;

public sealed class LocalFileStorage : IPrivateDocumentStorage
{
    public Task<PresignedUploadIntent> CreateUploadIntentAsync(string storageKey, string contentType, long maxSizeBytes, TimeSpan validity, CancellationToken cancellationToken = default)
    {
        var intent = new PresignedUploadIntent(
            storageKey,
            $"http://localhost/storage/{storageKey}",
            new Dictionary<string, string> { ["Content-Type"] = contentType },
            DateTimeOffset.UtcNow.Add(validity));
        return Task.FromResult(intent);
    }

    public Task<bool> ObjectExistsAsync(string storageKey, CancellationToken cancellationToken = default)
        => Task.FromResult(false);

    public Task<string> CreateDownloadUrlAsync(string storageKey, string? contentType, TimeSpan validity, CancellationToken cancellationToken = default)
        => Task.FromResult($"http://localhost/storage/{storageKey}");

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}