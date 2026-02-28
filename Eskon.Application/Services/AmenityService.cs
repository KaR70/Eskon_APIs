using Eskon.Application.Contracts.Amenity;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;
using Mapster;

namespace Eskon.Application.Services;

public class AmenityService : IAmenityService
{
    private readonly IUnitOfWork _unitOfWork;
    public AmenityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AmenityResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var amenities = await _unitOfWork.Amenities.GetAllAsync(cancellationToken);

        return amenities.Adapt<IEnumerable<AmenityResponse>>();
    }

    public async Task<Result<AmenityResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id, cancellationToken);

        return amenity is not null
            ? Result.Success(amenity.Adapt<AmenityResponse>()) 
            : Result.Failure<AmenityResponse>(AmenityErrors.AmenityNotFound);
    }

    public async Task<Result<AmenityResponse>> CreateAsync(AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var amenity = amenityRequest.Adapt<Amenity>();

        await _unitOfWork.Amenities.CreateAsync(amenity, cancellationToken);
        await _unitOfWork.CompleteAsync();

        var response = amenity.Adapt<AmenityResponse>();
        
        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id, cancellationToken);

        if (amenity is null)
        {
            return Result.Failure(AmenityErrors.AmenityNotFound);
        }

        _unitOfWork.Amenities.Delete(amenity);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(int id, AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var currentAmenity = await _unitOfWork.Amenities.GetByIdAsync(id, cancellationToken);

        if (currentAmenity is null)
        {
            return Result.Failure(AmenityErrors.AmenityNotFound);
        }

        amenityRequest.Adapt(currentAmenity);

        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }
}
