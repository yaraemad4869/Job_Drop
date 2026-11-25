using JobDrop.Application.Contracts.Storage;

namespace JobDrop.Infrastructure.Storage;

public sealed class FileSignatureValidator : IFileSignatureValidator
{
    private static readonly Dictionary<string, byte[][]> MagicBytes = new()
    {
        ["application/pdf"] = new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } },
        ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"] = new[] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } }
    };

    private static readonly HashSet<string> BlockedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".exe", ".dll", ".bat", ".cmd", ".sh", ".ps1", ".msi", ".scr", ".js", ".vbs", ".wsf"
    };

    public bool ValidateMagicBytes(string contentType, ReadOnlySpan<byte> header)
    {
        if (!MagicBytes.TryGetValue(contentType, out var signatures)) return true;
        foreach (var sig in signatures)
        {
            if (header.Length >= sig.Length && header.Slice(0, sig.Length).SequenceEqual(sig))
                return true;
        }
        return false;
    }

    public bool IsBlockedExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        return !string.IsNullOrEmpty(ext) && BlockedExtensions.Contains(ext);
    }
}