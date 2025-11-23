namespace Eskon_APIs.Errors;

public static class SavedListErrors
{
    public static readonly Error AlreadyExists = new("SavedList.AlreadyExists", "The house is already in the saved list.", StatusCodes.Status409Conflict);
    public static readonly Error NotFound = new("SavedList.NotFound", "The saved item was not found.", StatusCodes.Status404NotFound);
    public static readonly Error UserNotFound = new("User.NotFound", "The specified user does not exist.", StatusCodes.Status404NotFound);
    public static readonly Error HouseNotFound = new("House.NotFound", "The specified house does not exist.", StatusCodes.Status404NotFound);
}