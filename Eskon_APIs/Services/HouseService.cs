using Eskon_APIs.Contracts.House;
using Eskon_APIs.Entities;
using Eskon_APIs.Errors;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Eskon_APIs.Services;

public class HouseService : IHouseService
{
    private readonly ApplicationDbContext _context;

    public HouseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<HouseDetailResponse>> CreateAsync(CreateHouseRequest request, string ownerId, CancellationToken cancellationToken = default)
    {
        var locationExists = await _context.Location.AnyAsync(l => l.LocationId == request.LocationId, cancellationToken);

        if (!locationExists)
        {
            return Result.Failure<HouseDetailResponse>(HouseErrors.LocationNotFound);
        }
        
        var foundAmenitiesCount = await _context.Amenity.CountAsync(a => request.AmenityIds.Contains(a.AmenityId), cancellationToken);
        if (foundAmenitiesCount != request.AmenityIds.Count)
        {
            return Result.Failure<HouseDetailResponse>(HouseErrors.AmenitiesNotFound);
        }

        var house = request.Adapt<House>();
        house.OwnerId = ownerId;

        foreach (var amenityId in request.AmenityIds)
        {
            house.HouseAmenities.Add(new HouseAmenity
            {
                AmenityId = amenityId,
                House = house
            });
        }

        await _context.House.AddAsync(house, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var createdHouseWithDetails = await _context.House
            .Include(h => h.Owner)
            .Include(h => h.Location)
            .Include(h => h.MediaItems)
            .Include(h => h.HouseAmenities)
                .ThenInclude(ha => ha.Amenity)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.HouseId == house.HouseId, cancellationToken);

        if (createdHouseWithDetails is null)
        {
            return Result.Failure<HouseDetailResponse>(HouseErrors.CreationError);
        }

        var response = createdHouseWithDetails.Adapt<HouseDetailResponse>();

        response.isSavedByCurrrentUser = false;

        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id, string ownerId, CancellationToken cancellationToken = default)
    {
        var house = await _context.House
            .FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);

        if (house is null)
        {
            return Result.Failure(HouseErrors.HouseNotFound);
        }

        if (house.OwnerId != ownerId)
        {
            return Result.Failure(HouseErrors.NotOwner);
        }

        _context.House.Remove(house);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<List<HouseSummaryResponse>> GetAllAsync(string? CurrentUserId, CancellationToken cancellationToken = default)
    {
        var houses = await _context.House
            .AsNoTracking()
            .Include(h => h.MediaItems)
            .Include(h => h.Location)
            .ToListAsync(cancellationToken);

       
        var savedHousesIds = CurrentUserId != null
            ? await _context.SavedList
            .Where(SavedList => SavedList.UserId == CurrentUserId)
            .Select(SavedList => SavedList.HouseId)
            .ToListAsync(cancellationToken) : new List<int>();
        

        var houseSummaryResponses = houses.Adapt<List<HouseSummaryResponse>>();

        foreach (var house in houseSummaryResponses)
        {
            house.IsSavedByCurrentUser = savedHousesIds.Contains(house.HouseId);
        }

        return houseSummaryResponses;
    }

    public async Task<Result<HouseDetailResponse>> GetAsync(int id, string? currentUserId, CancellationToken cancellationToken = default)
    {
        var house = await _context.House
            .Include(h => h.Owner)
            .Include(h => h.Location)
            .Include(h => h.MediaItems)
            .Include(h => h.HouseAmenities)
                .ThenInclude(ha => ha.Amenity)
            .AsNoTracking() 
            .FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);

        if (house is null)
        {
            return Result.Failure<HouseDetailResponse>(HouseErrors.HouseNotFound);
        }

        var response = house.Adapt<HouseDetailResponse>();

        if (string.IsNullOrEmpty(currentUserId))
        {
            response.isSavedByCurrrentUser = false;
        }
        else
        {
            response.isSavedByCurrrentUser = await _context.SavedList
                .AnyAsync(s => s.HouseId == id && s.UserId == currentUserId, cancellationToken);
        }

        return Result.Success(response);
    }

    public async Task<List<HouseSummaryResponse>> GetMyListingsAsync(string CurrentUserId, CancellationToken cancellationToken = default)
    {
        var myHouses = await _context.House
            .AsNoTracking()
            .Where(h => h.OwnerId == CurrentUserId)
            .Include(h => h.MediaItems)
            .Include(h => h.Location)
            .ToListAsync(cancellationToken);


        var savedHousesIds = CurrentUserId != null
            ? await _context.SavedList
            .Where(SavedList => SavedList.UserId == CurrentUserId)
            .Select(SavedList => SavedList.HouseId)
            .ToListAsync(cancellationToken) : new List<int>();


        var houseSummaryResponses = myHouses.Adapt<List<HouseSummaryResponse>>();

        foreach (var house in houseSummaryResponses)
        {
            house.IsSavedByCurrentUser = savedHousesIds.Contains(house.HouseId);
        }

        return houseSummaryResponses;

    }

    public async Task<Result> UpdateAsync(int id, UpdateHouseRequest request, string ownerId, CancellationToken cancellationToken = default)
    {
        var house = await _context.House
            .Include(h => h.HouseAmenities)
            .FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);

        if (house is null)
        {
            return Result.Failure(HouseErrors.HouseNotFound);
        }

        if (house.OwnerId != ownerId)
        {
            return Result.Failure(HouseErrors.NotOwner);
        }

        var locationExists = await _context.Location.AnyAsync(l => l.LocationId == request.LocationId, cancellationToken);
        if (!locationExists)
        {
            return Result.Failure(HouseErrors.LocationNotFound);
        }

        var foundAmenitiesCount = await _context.Amenity.CountAsync(a => request.AmenityIds.Contains(a.AmenityId), cancellationToken);
        if (foundAmenitiesCount != request.AmenityIds.Count)
        {
            return Result.Failure(HouseErrors.AmenitiesNotFound);
        }

        request.Adapt(house);

        house.HouseAmenities.Clear();

        foreach (var amenityId in request.AmenityIds)
        {
            house.HouseAmenities.Add(new HouseAmenity { AmenityId = amenityId });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}