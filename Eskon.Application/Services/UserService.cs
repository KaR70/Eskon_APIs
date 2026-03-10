using Eskon.Application.Contracts.Users;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;

namespace Eskon.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserProfileResponse>> GetProfileAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.InvalidCredentials);

        // Check if user is in the Admin role
        bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        var profile = new UserProfileResponse(
            user.Email!,
            user.UserName ?? string.Empty,
            user.FirstName,
            user.LastName,
            isAdmin
        );

        return Result.Success(profile);
    }


    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, ErrorType.BadRequest));
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.UserName = request.UserName;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, ErrorType.BadRequest));
    }

    public async Task<Result> DeleteAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, ErrorType.BadRequest));
    }


}
