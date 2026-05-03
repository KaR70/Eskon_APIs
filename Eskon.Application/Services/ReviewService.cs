using Eskon.Application.Contracts.Review;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;

namespace Eskon.Application.Services;

public class ReviewService(IUnitOfWork unitOfWork) : IReviewService
{
    public async Task<Result<IEnumerable<ReviewResponse>>> GetMyReviewsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var reviews = await unitOfWork.Reviews.GetByUserIdAsync(userId, cancellationToken);

        var response = reviews.Select(r => new ReviewResponse
        {
            ReviewId = r.ReviewId,
            Stars = r.Stars,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            HouseId = r.HouseId,
            HouseTitle = r.House.Title,
            HouseCoverImageUrl = r.House.MediaItems
                .FirstOrDefault(m => m.IsCover)?.URL
                ?? r.House.MediaItems
                    .OrderBy(m => m.SortOrder)
                    .FirstOrDefault()?.URL
                ?? string.Empty
        });

        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<ReviewResponse>>> GetHouseReviewsAsync(
        int houseId,
        CancellationToken cancellationToken = default)
    {
        var houseExists = await unitOfWork.Houses.AnyAsync(h => h.HouseId == houseId, cancellationToken);

        if (!houseExists)
            return Result.Failure<IEnumerable<ReviewResponse>>(HouseErrors.HouseNotFound);

        var reviews = await unitOfWork.Reviews.GetByHouseIdAsync(houseId, cancellationToken);

        var response = reviews.Select(r => new ReviewResponse
        {
            ReviewId = r.ReviewId,
            Stars = r.Stars,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            HouseId = r.HouseId,
            HouseTitle = r.House.Title
        });

        return Result.Success(response);
    }

    public async Task<Result<ReviewResponse>> CreateAsync(
        string userId,
        int houseId,
        CreateReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var house = await unitOfWork.Houses.FirstOrDefaultAsync(h => h.HouseId == houseId, cancellationToken);

        if (house is null)
            return Result.Failure<ReviewResponse>(HouseErrors.HouseNotFound);

        if (house.OwnerId == userId)
            return Result.Failure<ReviewResponse>(ReviewErrors.CannotReviewOwnHouse);

        var alreadyReviewed = await unitOfWork.Reviews
            .AnyAsync(r => r.UserId == userId && r.HouseId == houseId, cancellationToken);

        if (alreadyReviewed)
            return Result.Failure<ReviewResponse>(ReviewErrors.AlreadyReviewed);

        var review = new Review
        {
            UserId = userId,
            HouseId = houseId,
            Stars = request.Stars,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Reviews.CreateAsync(review, cancellationToken);

        // Update house average rating
        await UpdateHouseRatingAsync(house, houseId, cancellationToken);

        await unitOfWork.CompleteAsync();

        return Result.Success(new ReviewResponse
        {
            ReviewId = review.ReviewId,
            Stars = review.Stars,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            HouseId = review.HouseId,
            HouseTitle = house.Title
        });
    }

    public async Task<Result> UpdateAsync(
        string userId,
        int reviewId,
        CreateReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var review = await unitOfWork.Reviews.GetReviewWithHouseAsync(reviewId, cancellationToken);

        if (review is null)
            return Result.Failure(ReviewErrors.ReviewNotFound);

        if (review.UserId != userId)
            return Result.Failure(ReviewErrors.NotOwner);

        review.Stars = request.Stars;
        review.Comment = request.Comment;

        // Recalculate house average rating
        await UpdateHouseRatingAsync(review.House, review.HouseId, cancellationToken);

        await unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        string userId,
        int reviewId,
        CancellationToken cancellationToken = default)
    {
        var review = await unitOfWork.Reviews.GetReviewWithHouseAsync(reviewId, cancellationToken);

        if (review is null)
            return Result.Failure(ReviewErrors.ReviewNotFound);

        if (review.UserId != userId)
            return Result.Failure(ReviewErrors.NotOwner);

        unitOfWork.Reviews.Delete(review);

        // Recalculate house average rating
        await UpdateHouseRatingAsync(review.House, review.HouseId, cancellationToken, excludeReviewId: reviewId);

        await unitOfWork.CompleteAsync();

        return Result.Success();
    }

    // Helper: recalculates and updates AverageRating + RatingCount on the house
    private async Task UpdateHouseRatingAsync(
        House house,
        int houseId,
        CancellationToken cancellationToken,
        int? excludeReviewId = null)
    {
        var allReviews = await unitOfWork.Reviews.GetByHouseIdAsync(houseId, cancellationToken);

        var validReviews = excludeReviewId.HasValue
            ? allReviews.Where(r => r.ReviewId != excludeReviewId.Value).ToList()
            : allReviews.ToList();

        house.RatingCount = validReviews.Count;
        house.AverageRating = validReviews.Count > 0
            ? Math.Round(validReviews.Average(r => r.Stars), 1)
            : null;
    }
}
