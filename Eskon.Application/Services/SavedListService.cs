using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;

namespace Eskon.Application.Services;

public class SavedListService(IUnitOfWork unitOfWork) : ISavedListService
{
    public async Task<Result> SaveAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        var houseExists = await unitOfWork.Houses.AnyAsync(h => h.HouseId == houseId, cancellationToken);

        if (!houseExists)
            return Result.Failure(SavedListErrors.HouseNotFound);

        var alreadySaved = await unitOfWork.SavedLists.AnyAsync(s => s.UserId == userId && s.HouseId == houseId, cancellationToken);

        if (alreadySaved)
            return Result.Failure(SavedListErrors.AlreadyExists);

        var saved = new SavedList
        {
            UserId = userId,
            HouseId = houseId,
            SavedAt = DateTime.UtcNow
        };

        await unitOfWork.SavedLists.CreateAsync(saved, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result> UnsaveAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        var saved = await unitOfWork.SavedLists.FirstOrDefaultAsync(s => s.UserId == userId && s.HouseId == houseId, cancellationToken);

        if (saved is null)
            return Result.Failure(SavedListErrors.NotFound);

        unitOfWork.SavedLists.Delete(saved);
        await unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result<bool>> IsSavedAsync(string userId, int houseId, CancellationToken cancellationToken = default)
    {
        var exists = await unitOfWork.SavedLists.AnyAsync(s => s.UserId == userId && s.HouseId == houseId, cancellationToken);

        return Result.Success(exists);
    }

    public async Task<Result<IEnumerable<House>>> GetSavedHousesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var houses = await unitOfWork.SavedLists.GetAllHousesAsync(userId, cancellationToken);

        return Result.Success(houses);
    }
}
