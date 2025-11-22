using Eskon_APIs.Contracts.Amenity;
using Eskon_APIs.Errors;

namespace Eskon_APIs.Services;

public class AmenityService : IAmenityService
{
    private readonly ApplicationDbContext _context;

    public AmenityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AmenityResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var amenities = await _context.Amenity.ToListAsync(cancellationToken);

        return amenities.Adapt<IEnumerable<AmenityResponse>>();
    }

    public async Task<Result<AmenityResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var amenity = await _context.Amenity.FindAsync(id, cancellationToken);

        return amenity is not null ? Result.Success(amenity.Adapt<AmenityResponse>()) : Result.Failure<AmenityResponse>(AmenityErrors.AmenityNotFound);
    }

    public async Task<AmenityResponse> AddAsync(AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var amenity = amenityRequest.Adapt<Amenity>();

        await _context.Amenity.AddAsync(amenity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return amenity.Adapt<AmenityResponse>();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var amenity = await _context.Amenity.FindAsync(id, cancellationToken);

        if (amenity is null)
        {
            return Result.Failure(AmenityErrors.AmenityNotFound);
        }

        _context.Remove(amenity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(int id, AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var currentAmenity = await _context.Amenity.FindAsync(id);

        if (currentAmenity is null)
        {
            return Result.Failure(AmenityErrors.AmenityNotFound);
        }

        amenityRequest.Adapt(currentAmenity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
