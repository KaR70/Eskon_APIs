using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;

namespace Eskon.Application.Services
{
    public interface ISavedListService
    {
        Task<Result> SaveAsync(string userId, int houseId, CancellationToken cancellationToken = default);
        Task<Result> UnsaveAsync(string userId, int houseId, CancellationToken cancellationToken = default);
        Task<Result<bool>> IsSavedAsync(string userId, int houseId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<House>>> GetSavedHousesAsync(string userId, CancellationToken cancellationToken = default);
    }
}
