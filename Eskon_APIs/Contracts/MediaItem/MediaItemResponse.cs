namespace Eskon_APIs.Contracts.MediaItem;

public class MediaItemResponse
{
    public int MediaId { get; set; }
    public string URL { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
}
