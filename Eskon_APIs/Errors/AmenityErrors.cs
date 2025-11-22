namespace Eskon_APIs.Errors;

public static class AmenityErrors
{
    public static readonly Error AmenityNotFound = new("Amentiy.NotFound", "No Amenity was found with the given ID", StatusCodes.Status404NotFound);
}
