namespace Eskon.Application.Contracts.Users;

public record UserProfileResponse(
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    bool IsAdmin,
    string? ProfilePictureUrl
);