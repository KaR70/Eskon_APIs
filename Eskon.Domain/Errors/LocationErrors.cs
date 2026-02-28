namespace Eskon.Domain.Errors;

public static class LocationErrors
{
    public static readonly Error LocationNotFound = new(
        (string)(string)"Location.NotFound", (string)(string)"The location with the specified ID was not found", ErrorType.NotFound);
}
