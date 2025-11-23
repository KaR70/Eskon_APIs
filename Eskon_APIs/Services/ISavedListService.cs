namespace Eskon_APIs.Services
{
    public interface ISavedListService
    {
        Task<Result> SaveAsync(string userId, int houseId, CancellationToken cancellationToken = default);
        Task<Result> UnsaveAsync(string userId, int houseId, CancellationToken cancellationToken = default);
        Task<Result<bool>> IsSavedAsync(string userId, int houseId, CancellationToken cancellationToken = default);
        Task<Result<List<House>>> GetSavedHousesAsync(string userId, CancellationToken cancellationToken = default);
    }
}
