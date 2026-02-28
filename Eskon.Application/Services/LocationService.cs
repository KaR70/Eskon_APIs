using Eskon.Application.Contracts.Location;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;
using Mapster;

namespace Eskon.Application.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    public LocationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LocationResponse>> CreateAsync(LocationRequest locationRequest, CancellationToken cancellationToken = default)
    {
        var location = locationRequest.Adapt<Location>();

        await _unitOfWork.Locations.CreateAsync(location, cancellationToken);
        await _unitOfWork.CompleteAsync();

        return Result.Success(location.Adapt<LocationResponse>());
    }

    public async Task<IEnumerable<LocationResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _unitOfWork.Locations.GetAllAsync(cancellationToken);
        return locations.Adapt<IEnumerable<LocationResponse>>();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id, cancellationToken);

        if (location is null)
            return Result.Failure(LocationErrors.LocationNotFound);

        _unitOfWork.Locations.Delete(location);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result<LocationResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id, cancellationToken);

        return location is not null 
            ? Result.Success(location.Adapt<LocationResponse>())
            : Result.Failure<LocationResponse>(LocationErrors.LocationNotFound);
    }

    public async Task<Result> UpdateAsync(int id, LocationRequest locationRequest, CancellationToken cancellationToken = default)
    {
        var currentLocation = await _unitOfWork.Locations.GetByIdAsync(id, cancellationToken);

        if (currentLocation is null)
        {
            return Result.Failure(LocationErrors.LocationNotFound);
        }

        locationRequest.Adapt(currentLocation);

        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }
}
