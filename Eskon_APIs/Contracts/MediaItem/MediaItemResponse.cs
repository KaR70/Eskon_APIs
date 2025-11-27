namespace Eskon_APIs.Contracts.MediaItem;

/// <summary>
/// Response model containing media item (image) information.
/// </summary>
public class MediaItemResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the media item.
    /// </summary>
    public int MediaId { get; set; }

    /// <summary>
    /// Gets or sets the URL where the image is stored and can be accessed.
    /// </summary>
    /// <remarks>
    /// This is the direct URL to the image file. Can be used in image tags or API responses.
    /// Example: "https://api.example.com/images/houses/123/image-abc123.jpg"
    /// </remarks>
    public string URL { get; set; } = null!;

    /// <summary>
    /// Gets or sets the sort order of the image within the house's image gallery.
    /// </summary>
    /// <remarks>
    /// Used to determine the display order of images when showing a gallery of house images.
    /// Lower numbers appear first in the gallery.
    /// Example: 0, 1, 2, 3, etc.
    /// </remarks>
    public int SortOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this image is the cover/thumbnail image for the house.
    /// </summary>
    /// <remarks>
    /// Only one image per house should have IsCover set to true.
    /// The cover image is displayed as the main thumbnail in house listings.
    /// Can be changed using the set-cover endpoint.
    /// </remarks>
    public bool IsCover { get; set; }
}
