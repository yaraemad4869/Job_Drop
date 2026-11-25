namespace JobDrop.Application.Contracts.Storage;

public interface IFileSignatureValidator
{
    bool ValidateMagicBytes(string contentType, ReadOnlySpan<byte> header);
    bool IsBlockedExtension(string fileName);
}