using Eskon.Application.Contracts.Amenity;
using Eskon.Domain.Abstraction;

namespace Eskon.Application.Services;

public interface IAmenityService
{
    Task<IEnumerable<AmenityResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<AmenityResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<AmenityResponse>> CreateAsync(AmenityRequest amenityRequest, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, AmenityRequest amenityRequest, CancellationToken cancellationToken = default);
}
