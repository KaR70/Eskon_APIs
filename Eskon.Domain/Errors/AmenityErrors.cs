namespace Eskon.Domain.Errors;

public static class AmenityErrors
{
    public static readonly Error AmenityNotFound = 
        new((string)(string)"Amentiy.NotFound", (string)(string)"No Amenity was found with the given ID", ErrorType.NotFound);
}
