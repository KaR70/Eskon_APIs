using Eskon.Domain.Entities;

namespace Eskon.Domain.Interfaces;

public interface IHouseRepository : IBaseRepository<House>
{
    Task<House?> GetHouseWithDetailsAsync(int houseId, CancellationToken cancellationToken);
    Task<IEnumerable<House>> GetHousesSummariesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<House>> GetHousesByOwnerIdAsync(string ownerId, CancellationToken cancellationToken);
    Task<House?> GetHouseWithAmenitiesAsync(int id, CancellationToken cancellationToken);
    Task<House?> GetHouseWithMediaItemsAsync(int id, CancellationToken cancellationToken);
}