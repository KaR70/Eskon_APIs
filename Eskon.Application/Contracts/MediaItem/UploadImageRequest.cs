using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Eskon.Application.Contracts.MediaItem;

public class UploadImageRequest
{
    [Required]
    public IFormFile File { get; set; }
}
