using Eskon.Domain.Entities;

namespace Eskon.Domain.Interfaces;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<IEnumerable<Review>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetByHouseIdAsync(int houseId, CancellationToken cancellationToken = default);
    Task<Review?> GetByUserAndHouseAsync(string userId, int houseId, CancellationToken cancellationToken = default);
    Task<Review?> GetReviewWithHouseAsync(int reviewId, CancellationToken cancellationToken = default);
}
