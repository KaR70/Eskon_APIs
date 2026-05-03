namespace Eskon.Application.Contracts.Review;

public class ReviewResponse
{
    public int ReviewId { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    // House info so user knows which house the review belongs to
    public int HouseId { get; set; }
    public string HouseTitle { get; set; } = null!;
    public string HouseCoverImageUrl { get; set; } = string.Empty;
}
