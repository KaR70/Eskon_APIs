using Eskon.Domain.Entities;

namespace Eskon.Domain.Interfaces;

public interface IMediaItemRepository : IBaseRepository<MediaItem>
{
    Task<MediaItem?> GetMediaItemWithHouseAsync(int id, int houseId, CancellationToken cancellationToken);
    Task<MediaItem?> GetNextAvailableCoverImageAsync(int id, int houseId, CancellationToken cancellationToken);
    Task<IEnumerable<MediaItem>> GetByHouseIdAsync(int houseId, CancellationToken cancellationToken = default);
}