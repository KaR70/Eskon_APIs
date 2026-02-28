namespace Eskon.Domain.Errors;

public static class SavedListErrors
{
    public static readonly Error AlreadyExists 
        = new((string)(string)"SavedList.AlreadyExists", (string)(string)"The house is already in the saved list.", ErrorType.Conflict);
    public static readonly Error NotFound 
        = new((string)(string)"SavedList.NotFound", (string)(string)"The saved item was not found.", ErrorType.NotFound);
    public static readonly Error UserNotFound 
        = new((string)(string)"User.NotFound", (string)(string)"The specified user does not exist.", ErrorType.NotFound);
    public static readonly Error HouseNotFound 
        = new((string)(string)"House.NotFound", (string)(string)"The specified house does not exist.", ErrorType.NotFound);
}