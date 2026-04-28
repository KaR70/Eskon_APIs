using Eskon.Application.Contracts.Users;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Eskon.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserService(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
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
            isAdmin,
            user.ProfilePictureUrl
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

    public async Task<Result<string>> UploadProfilePictureAsync(string userId, IFormFile file, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<string>(UserErrors.NotFound);

        if (file == null || file.Length == 0)
            return Result.Failure<string>(UserErrors.NoFile);

        if (file.Length > 5 * 1024 * 1024)
            return Result.Failure<string>(UserErrors.FileTooLarge);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return Result.Failure<string>(UserErrors.InvalidFileType);

        var webRootPath = _webHostEnvironment?.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        // Delete old picture if exists
        if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
        {
            var oldPath = Path.Combine(webRootPath, user.ProfilePictureUrl.TrimStart('/'));
            if (File.Exists(oldPath))
                File.Delete(oldPath);
        }

        var fileName = $"avatar_{userId}_{Guid.NewGuid()}{extension}";
        var folderPath = Path.Combine(webRootPath, "images", "avatars");

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        user.ProfilePictureUrl = $"/images/avatars/{fileName}";

        await _userManager.UpdateAsync(user);

        return Result.Success(user.ProfilePictureUrl);
    }

    public async Task<Result> DeleteProfilePictureAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        if (string.IsNullOrEmpty(user.ProfilePictureUrl))
            return Result.Failure(UserErrors.NoPictureToDelete);

        var webRootPath = _webHostEnvironment?.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var physicalPath = Path.Combine(webRootPath, user.ProfilePictureUrl.TrimStart('/'));

        if (File.Exists(physicalPath))
            File.Delete(physicalPath);

        user.ProfilePictureUrl = null;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }


}
