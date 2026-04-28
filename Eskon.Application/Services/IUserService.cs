using Eskon.Application.Contracts.Users;
using Eskon.Domain.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Eskon.Application.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<Result> DeleteAccountAsync(string userId);
    Task<Result<string>> UploadProfilePictureAsync(string userId, IFormFile file, CancellationToken cancellationToken = default);
    Task<Result> DeleteProfilePictureAsync(string userId, CancellationToken cancellationToken = default);
}
