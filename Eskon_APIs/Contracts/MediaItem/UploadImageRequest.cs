using System.ComponentModel.DataAnnotations;

namespace Eskon_APIs.Contracts.MediaItem;

public class UploadImageRequest
{
    [Required]
    public IFormFile File { get; set; }
}
