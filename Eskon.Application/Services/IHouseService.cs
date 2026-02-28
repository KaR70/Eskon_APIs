using Eskon.Application.Contracts.House;
using Eskon.Domain.Abstraction;

namespace Eskon.Application.Services;

public interface IHouseService
{
    Task<List<HouseSummaryResponse>> GetAllAsync(string? CurrentUserId, CancellationToken cancellationToken = default);
    Task<List<HouseSummaryResponse>> GetMyListingsAsync(string CurrentUserId, CancellationToken cancellationToken = default);
    Task<Result<HouseDetailResponse>> GetAsync(int id, string? currentUserId, CancellationToken cancellationToken = default);
    Task<Result<HouseDetailResponse>> CreateAsync(CreateHouseRequest request, string ownerId, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, UpdateHouseRequest request, string ownerId, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, string ownerId, CancellationToken cancellationToken = default);
}
