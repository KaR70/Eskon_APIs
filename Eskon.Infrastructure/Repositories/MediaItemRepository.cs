using Eskon.Domain.Interfaces;

namespace Eskon.Infrastructure.Repositories;

public class MediaItemRepository(ApplicationDbContext context) : BaseRepository<MediaItem>(context), IMediaItemRepository
{
    public async Task<MediaItem?> GetMediaItemWithHouseAsync(int id, int houseId, CancellationToken cancellationToken)
    {
        return await context.MediaItem
            .Include(m => m.House) 
            .FirstOrDefaultAsync(m => m.MediaId == id && m.HouseId == houseId, cancellationToken);
    }

    public async Task<MediaItem?> GetNextAvailableCoverImageAsync(int id, int houseId, CancellationToken cancellationToken)
    {
        return await context.MediaItem
            .Where(m => m.HouseId == houseId && m.MediaId != id)
            .OrderBy(m => m.SortOrder)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<MediaItem>> GetByHouseIdAsync(int houseId, CancellationToken cancellationToken = default)
    {
        return await context.MediaItem
            .Where(m => m.HouseId == houseId)
            .ToListAsync(cancellationToken);
    }
}