using Eskon.Domain.Entities;

namespace Eskon.Domain.Interfaces;

public interface ISavedListRepository : IBaseRepository<SavedList>
{
    Task<IEnumerable<House>> GetAllHousesAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<int>> GetAllUserSavedHousesAsync(string userId, CancellationToken cancellationToken = default);
}