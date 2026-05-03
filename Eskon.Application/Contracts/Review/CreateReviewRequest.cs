namespace Eskon.Application.Contracts.Review;

public class CreateReviewRequest
{
    public int Stars { get; set; }
    public string? Comment { get; set; }
}
