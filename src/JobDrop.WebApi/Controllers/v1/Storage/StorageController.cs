using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1.Storage;

[ApiController]
[Route("api/v1/storage")]
[Authorize]
public sealed class StorageController : ControllerBase
{
    [HttpPost("upload-intent")]
    public IActionResult UploadIntent([FromBody] object request)
        => Ok(new { UploadUrl = "presigned-url-skeleton", StorageKey = "blobs/..." });

    [HttpPost("upload-confirm")]
    public IActionResult UploadConfirm([FromBody] object request)
        => Ok(new { Status = "Validated" });

    [HttpGet("download-url/{storageKey}")]
    public IActionResult DownloadUrl(string storageKey)
        => Ok(new { DownloadUrl = "presigned-get-skeleton" });
}