using Eskon_APIs.Contracts.Location;

namespace Eskon_APIs.Services;

public interface ILocationService
{
    Task<Result<LocationResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<LocationResponse>> AddAsync(LocationRequest locationRequest, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, LocationRequest locationRequest, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
