namespace Eskon.Domain.Errors;
public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new((string)(string)"User.InvalidCredentials", (string)(string)"Invalid email/password", ErrorType.Unauthorized);

    public static readonly Error InvalidJwtToken =
        new((string)(string)"User.InvalidJwtToken", (string)(string)"Invalid Jwt token", ErrorType.Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new((string)(string)"User.InvalidRefreshToken", (string)(string)"Invalid refresh token", ErrorType.Unauthorized);

    public static readonly Error DuplicatedEmail =
        new((string)(string)"User.DuplicatedEmail", (string)(string)"Another user with the same email is already exists", ErrorType.Conflict);

    public static readonly Error EmailNotConfirmed =
        new((string)(string)"User.EmailNotConfirmed", (string)(string)"Email is not confirmed", ErrorType.Unauthorized);

    public static readonly Error InvalidCode =
        new((string)(string)"User.InvalidCode", (string)(string)"Invalid code", ErrorType.Unauthorized);

    public static readonly Error DuplicatedConfirmation =
        new((string)(string)"User.DuplicatedConfirmation", (string)(string)"Email already confirmed", ErrorType.BadRequest);

    public static readonly Error NotFound =
        new("User.NotFound", "User not found", ErrorType.NotFound);
}
