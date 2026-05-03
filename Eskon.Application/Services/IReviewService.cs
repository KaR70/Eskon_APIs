using Eskon.Application.Contracts.Review;
using Eskon.Domain.Abstraction;

namespace Eskon.Application.Services;

public interface IReviewService
{
    Task<Result<IEnumerable<ReviewResponse>>> GetMyReviewsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ReviewResponse>>> GetHouseReviewsAsync(int houseId, CancellationToken cancellationToken = default);
    Task<Result<ReviewResponse>> CreateAsync(string userId, int houseId, CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string userId, int reviewId, CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string userId, int reviewId, CancellationToken cancellationToken = default);
}
