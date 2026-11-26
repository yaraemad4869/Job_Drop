using JobDrop.Application.Contracts.Storage;

namespace JobDrop.Infrastructure.Storage;

public sealed class PrivateDocumentStorage : IPrivateDocumentStorage
{
    public Task<PresignedUploadIntent> CreateUploadIntentAsync(
        string targetKind, 
        string mimeType, 
        long maxSizeBytes, 
        TimeSpan validity, 
        CancellationToken ct)
    {
        // TODO: Generate actual presigned URL from S3/R2/Blob
        var storageKey = $"blobs/{targetKind}/{Guid.NewGuid()}.pdf";
        var requiredHeaders = new Dictionary<string, string>
        {
            ["Content-Type"] = mimeType,
            ["x-amz-server-side-encryption"] = "AES256"
        };
        
        // Constructor: PresignedUploadIntent(string storageKey, string uploadUrl, 
        //                                   IDictionary<string, string> requiredHeaders, 
        //                                   DateTimeOffset expiresAtUtc)
        return Task.FromResult(new PresignedUploadIntent(
            StorageKey: storageKey,
            UploadUrl: "presigned-put-url-skeleton",
            RequiredHeaders: requiredHeaders,
            ExpiresAtUtc: DateTimeOffset.UtcNow.Add(validity)
        ));
    }

    public Task<bool> ObjectExistsAsync(string storageKey, CancellationToken ct)
    {
        // TODO: Check S3/R2/Blob
        return Task.FromResult(true);
    }

    public Task<string> CreateDownloadUrlAsync(
        string storageKey, 
        string? contentType, 
        TimeSpan validity, 
        CancellationToken ct)
    {
        // TODO: Generate actual presigned GET URL
        return Task.FromResult("presigned-get-url-skeleton");
    }

    public Task DeleteAsync(string storageKey, CancellationToken ct)
    {
        // TODO: Delete from S3/R2/Blob
        return Task.CompletedTask;
    }
}
