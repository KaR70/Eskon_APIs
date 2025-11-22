using Eskon_APIs.Contracts.Amenity;

namespace Eskon_APIs.Services;

public interface IAmenityService
{
    Task<IEnumerable<AmenityResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<AmenityResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<AmenityResponse> AddAsync(AmenityRequest amenityRequest, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, AmenityRequest amenityRequest, CancellationToken cancellationToken = default);
}
