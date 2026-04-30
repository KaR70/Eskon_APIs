using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Enums;
using Eskon.Domain.Errors;
using Mapster;

namespace Eskon.Application.Services;

public class HouseService(IUnitOfWork unitOfWork) : IHouseService
{
    public async Task<Result<HouseDetailResponse>> CreateAsync(CreateHouseRequest request, string ownerId, CancellationToken cancellationToken = default)
    {
        var locationExists = await unitOfWork.Locations.AnyAsync(l => l.LocationId == request.LocationId, cancellationToken);

        if (!locationExists)
        {
            return Result.Failure<HouseDetailResponse>(HouseErrors.LocationNotFound);
        }
        
        var foundAmenitiesCount = await unitOfWork.Amenities.CountAsync(a => request.AmenityIds.Contains(a.AmenityId), cancellationToken);
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

        await unitOfWork.Houses.CreateAsync(house, cancellationToken);
        await unitOfWork.CompleteAsync();

        var createdHouseWithDetails = await unitOfWork.Houses.GetHouseWithDetailsAsync(house.HouseId, cancellationToken);

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
        var house = await unitOfWork.Houses.FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);

        if (house is null)
        {
            return Result.Failure(HouseErrors.HouseNotFound);
        }

        if (house.OwnerId != ownerId)
        {
            return Result.Failure(HouseErrors.NotOwner);
        }

        unitOfWork.Houses.Delete(house);

        await unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result<HomeResponse>> GetAllAsync(string? CurrentUserId, CancellationToken cancellationToken = default)
    {
        var houses = await unitOfWork.Houses.GetHousesSummariesAsync(cancellationToken);
       
        var savedHousesIds = CurrentUserId != null
            ? await unitOfWork.SavedLists.GetAllUserSavedHousesAsync(CurrentUserId, cancellationToken) : new List<int>();
        
        var houseSummaryResponses = houses.Adapt<List<HouseSummaryResponse>>();

        foreach (var house in houseSummaryResponses)
        {
            house.IsSavedByCurrentUser = savedHousesIds.Contains(house.HouseId);
        }

        var apartments = houseSummaryResponses
            .Where(x => x.Type == nameof(HouseType.Apartment))
            .ToList();
        
        var villas = houseSummaryResponses
            .Where(x => x.Type == nameof(HouseType.Villa))
            .ToList();
        
        var hotels = houseSummaryResponses
            .Where(x => x.Type == nameof(HouseType.Hotel))
            .ToList();
        
        return Result.Success(new HomeResponse(apartments, hotels, villas));
    }

    public async Task<Result<HouseDetailResponse>> GetAsync(int id, string? currentUserId, CancellationToken cancellationToken = default)
    {
        var house = await unitOfWork.Houses.FirstOrDefaultAsync(h => h.HouseId == id, cancellationToken);

        if (house is null)
            return Result.Failure<HouseDetailResponse>(HouseErrors.HouseNotFound);

        var response = house.Adapt<HouseDetailResponse>();

        if (string.IsNullOrEmpty(currentUserId))
        {
            response.isSavedByCurrrentUser = false;
        }
        else
        {
            response.isSavedByCurrrentUser =
                await unitOfWork.SavedLists
                    .AnyAsync(s => s.HouseId == id && s.UserId == currentUserId, cancellationToken);
        }

        return Result.Success(response);
    }

    public async Task<List<HouseSummaryResponse>> GetMyListingsAsync(string CurrentUserId, CancellationToken cancellationToken = default)
    {
        var myHouses = await unitOfWork.Houses.GetHousesByOwnerIdAsync(CurrentUserId, cancellationToken);
        
        var savedHousesIds = CurrentUserId != null
            ? await unitOfWork.SavedLists.GetAllUserSavedHousesAsync(CurrentUserId, cancellationToken) : new List<int>();


        var houseSummaryResponses = myHouses.Adapt<List<HouseSummaryResponse>>();

        foreach (var house in houseSummaryResponses)
        {
            house.IsSavedByCurrentUser = savedHousesIds.Contains(house.HouseId);
        }

        return houseSummaryResponses;

    }

    public async Task<Result> UpdateAsync(int id, UpdateHouseRequest request, string ownerId, CancellationToken cancellationToken = default)
    {
        var house = await unitOfWork.Houses.GetHouseWithAmenitiesAsync(id, cancellationToken);

        if (house is null)
            return Result.Failure(HouseErrors.HouseNotFound);

        if (house.OwnerId != ownerId)
            return Result.Failure(HouseErrors.NotOwner);

        var locationExists = await unitOfWork.Locations.AnyAsync(l => l.LocationId == request.LocationId, cancellationToken);
        
        if (!locationExists)
            return Result.Failure(HouseErrors.LocationNotFound);

        var foundAmenitiesCount = await unitOfWork.Amenities.CountAsync(a => request.AmenityIds.Contains(a.AmenityId), cancellationToken);
        
        if (foundAmenitiesCount != request.AmenityIds.Count)
            return Result.Failure(HouseErrors.AmenitiesNotFound);

        request.Adapt(house);

        house.HouseAmenities.Clear();

        foreach (var amenityId in request.AmenityIds)
        {
            house.HouseAmenities.Add(new HouseAmenity { AmenityId = amenityId });
        }

        await unitOfWork.CompleteAsync();

        return Result.Success();
    }
}