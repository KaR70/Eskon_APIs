namespace Eskon_APIs.Errors;

public static class LocationErrors
{
    public static readonly Error LocationNotFound = new(
        "Location.NotFound", "The location with the specified ID was not found", StatusCodes.Status404NotFound);
}
