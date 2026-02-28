using System.Linq.Expressions;
using Eskon.Domain.Interfaces;

namespace Eskon.Infrastructure.Repositories;

public class HouseRepository(ApplicationDbContext context) : BaseRepository<House>(context), IHouseRepository
{
    public async Task<House?> GetHouseWithDetailsAsync(int houseId, CancellationToken cancellationToken)
        => await Context.House
                .Include(h => h.Owner)
                .Include(h => h.Location)
                .Include(h => h.MediaItems)
                .Include(h => h.HouseAmenities)
                .ThenInclude(ha => ha.Amenity)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.HouseId == houseId, cancellationToken);

    public async Task<IEnumerable<House>> GetHousesSummariesAsync(CancellationToken cancellationToken)
       => await Context.House
            .AsNoTracking()
            .Include(h => h.MediaItems)
            .Include(h => h.Location)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<House>> GetHousesByOwnerIdAsync(string ownerId, CancellationToken cancellationToken)
    {
        return await context.House
            .AsNoTracking()
            .Where(h => h.OwnerId == ownerId)
            .Include(h => h.MediaItems)
            .Include(h => h.Location)
            .ToListAsync(cancellationToken);
    }

    public async Task<House?> GetHouseWithAmenitiesAsync(int id, CancellationToken cancellationToken)
    {
        return await context.House
            .Include(h => h.HouseAmenities)
            .FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);
    }

    public async Task<House?> GetHouseWithMediaItemsAsync(int id, CancellationToken cancellationToken)
    {
        return await context.House
            .Include(h => h.MediaItems)
            .FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);
    }
}