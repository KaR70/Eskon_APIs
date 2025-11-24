using Eskon_APIs.Contracts.House;

namespace Eskon_APIs.Services;

public interface IHouseService
{
    Task<List<HouseSummaryResponse>> GetAllAsync(string? CurrentUserId, CancellationToken cancellationToken = default);
    Task<List<HouseSummaryResponse>> GetMyListingsAsync(string CurrentUserId, CancellationToken cancellationToken = default);
    Task<Result<HouseDetailResponse>> GetAsync(int id, string? currentUserId, CancellationToken cancellationToken = default);
    Task<Result<HouseDetailResponse>> CreateAsync(CreateHouseRequest request, string ownerId, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, UpdateHouseRequest request, string ownerId, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, string ownerId, CancellationToken cancellationToken = default);
}
