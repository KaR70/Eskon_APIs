using Eskon.Application.Contracts.MediaItem;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Eskon.Application.Services;

public class MediaService : IMediaService
{
    private readonly IWebHostEnvironment _webHostingEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    public MediaService(IWebHostEnvironment webHostingEnvironment, IUnitOfWork unitOfWork)
    {
        _webHostingEnvironment = webHostingEnvironment;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> DeleteImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default)
    {
        var mediaItem = await _unitOfWork.MediaItems.GetMediaItemWithHouseAsync(mediaItemId, houseId, cancellationToken);

        if (mediaItem is null)
            return Result.Failure(MediaItemErrors.NotFound);

        if (mediaItem.House.OwnerId != ownerId)
            return Result.Failure(HouseErrors.NotOwner);

        var physicalPath = Path.Combine(_webHostingEnvironment.WebRootPath, mediaItem.URL.TrimStart('/'));
        
        if (File.Exists(physicalPath))
            File.Delete(physicalPath);

        _unitOfWork.MediaItems.Delete(mediaItem);

        if (mediaItem.IsCover)
        {
            var newCoverImage = await _unitOfWork.MediaItems.GetNextAvailableCoverImageAsync(mediaItemId, mediaItem.HouseId, cancellationToken);

            if (newCoverImage is not null)
            {
                newCoverImage.IsCover = true;
            }
        }

        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result> SetCoverImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default)
    {
        var house = await _unitOfWork.Houses.GetHouseWithMediaItemsAsync(houseId, cancellationToken);

        if (house is null)
            return Result.Failure(HouseErrors.HouseNotFound);

        if (house.OwnerId != ownerId)
            return Result.Failure(HouseErrors.NotOwner);

        var newCover = house.MediaItems.FirstOrDefault(m => m.MediaId == mediaItemId);
        
        if (newCover is null)
            return Result.Failure(MediaItemErrors.NotFound);

        foreach (var image in house.MediaItems)
            image.IsCover = (image.MediaId == mediaItemId);

        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result<MediaItemResponse>> UploadImageForHouseAsync(int houseId, string ownerId, IFormFile imageFile, CancellationToken cancellationToken = default)
    {
        var house = await _unitOfWork.Houses.FirstOrDefaultAsync(h => h.HouseId == houseId, cancellationToken);

        if (house is null)
            return Result.Failure<MediaItemResponse>(HouseErrors.HouseNotFound);
        
        if (house.OwnerId != ownerId)
            return Result.Failure<MediaItemResponse>(HouseErrors.NotOwner);

        if (imageFile == null || imageFile.Length == 0)
            return Result.Failure<MediaItemResponse>(MediaItemErrors.NoFile);
        
        if (imageFile.Length > 5 * 1024 * 1024)
            return Result.Failure<MediaItemResponse>(MediaItemErrors.FileTooLarge);

        var fileExtension = Path.GetExtension((string?)imageFile.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var uploadsFolderPath = Path.Combine(_webHostingEnvironment.WebRootPath, "images");
        var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

        Directory.CreateDirectory(uploadsFolderPath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream, cancellationToken);
        }

        var existingImages = await _unitOfWork.MediaItems.GetByHouseIdAsync(houseId, cancellationToken);

        var isFirstImage = !existingImages.Any();

        var nextSortOrder = existingImages.Any()
            ? existingImages.Max(m => m.SortOrder) + 1
            : 0;

        var mediaItem = new MediaItem
        {
            HouseId = houseId,
            URL = $"/images/{uniqueFileName}",
            IsCover = isFirstImage,
            SortOrder = nextSortOrder
        };

        await _unitOfWork.MediaItems.CreateAsync(mediaItem, cancellationToken);
        await _unitOfWork.CompleteAsync();

        var response = mediaItem.Adapt<MediaItemResponse>();
        return Result.Success(response);
    }
}
