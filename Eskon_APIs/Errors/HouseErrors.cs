namespace Eskon_APIs.Errors;

public static class HouseErrors
{
    public static readonly Error LocationNotFound = new("House.LocationNotFound", "The specified location does not exist.", StatusCodes.Status404NotFound);
    public static readonly Error AmenitiesNotFound = new("House.AmenitiesNotFound", "One or more of the specified amenities do not exist.", StatusCodes.Status404NotFound);
    public static readonly Error CreationError = new("House.CreationError", "Could not retrieve the house details after creation.", StatusCodes.Status500InternalServerError);
    public static readonly Error HouseNotFound = new("House.NotFound", "The house with the specified ID was not found.", StatusCodes.Status404NotFound);
    public static readonly Error NotOwner = new("House.NotOwner", "You are not authorized to perform this action on the specified house.", StatusCodes.Status403Forbidden);
}
