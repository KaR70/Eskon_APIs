using Eskon.Application.Contracts.MediaItem;
using Eskon.Domain.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Eskon.Application.Services;

public interface IMediaService
{
    Task<Result<MediaItemResponse>> UploadImageForHouseAsync(int houseId, string ownerId, IFormFile imageFile, CancellationToken cancellationToken = default);
    Task<Result> DeleteImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default);
    Task<Result> SetCoverImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default);
}
