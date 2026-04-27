using Eskon.Application.Contracts.Users;
using Eskon.Domain.Abstraction;

namespace Eskon.Application.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<Result> DeleteAccountAsync(string userId);
}
