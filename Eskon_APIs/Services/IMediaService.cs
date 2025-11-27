using Eskon_APIs.Contracts.MediaItem;

namespace Eskon_APIs.Services;

public interface IMediaService
{
    Task<Result<MediaItemResponse>> UploadImageForHouseAsync(int houseId, string ownerId, IFormFile imageFile, CancellationToken cancellationToken = default);
    Task<Result> DeleteImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default);
    Task<Result> SetCoverImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default);
}
