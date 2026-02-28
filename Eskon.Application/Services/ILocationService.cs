using Eskon.Application.Contracts.Location;
using Eskon.Domain.Abstraction;

namespace Eskon.Application.Services;

public interface ILocationService
{
    Task<IEnumerable<LocationResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<LocationResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<LocationResponse>> CreateAsync(LocationRequest locationRequest, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, LocationRequest locationRequest, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
