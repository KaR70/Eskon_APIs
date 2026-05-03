using Eskon.Domain.Entities;
using Eskon.Domain.Interfaces;

namespace Eskon.Infrastructure.Repositories;

public class ReviewRepository(ApplicationDbContext context)
    : BaseRepository<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Review
            .Where(r => r.UserId == userId)
            .Include(r => r.House)
                .ThenInclude(h => h.MediaItems)
            .OrderByDescending(r => r.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetByHouseIdAsync(int houseId, CancellationToken cancellationToken = default)
    {
        return await context.Review
            .Where(r => r.HouseId == houseId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Review?> GetByUserAndHouseAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        return await context.Review
            .FirstOrDefaultAsync(r => r.UserId == userId && r.HouseId == houseId, cancellationToken);
    }

    public async Task<Review?> GetReviewWithHouseAsync(int reviewId, CancellationToken cancellationToken = default)
    {
        return await context.Review
            .Include(r => r.House)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId, cancellationToken);
    }
}
