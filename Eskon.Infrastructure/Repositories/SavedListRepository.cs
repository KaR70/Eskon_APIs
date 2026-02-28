using Eskon.Domain.Interfaces;

namespace Eskon.Infrastructure.Repositories;

public class SavedListRepository(ApplicationDbContext context) : BaseRepository<SavedList>(context), ISavedListRepository
{
    public async Task<IEnumerable<House>> GetAllHousesAsync(string userId, CancellationToken cancellationToken = default)
    {
         return await context.SavedList
            .Where(s => s.UserId == userId)
            .Select(s => s.House)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<int>> GetAllUserSavedHousesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.SavedList
            .Where(SavedList => SavedList.UserId == userId)
            .Select(SavedList => SavedList.HouseId)
            .ToListAsync(cancellationToken);
    }
}