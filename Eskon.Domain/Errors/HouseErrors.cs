namespace Eskon.Domain.Errors;

public static class HouseErrors
{
    public static readonly Error LocationNotFound = 
        new((string)(string)"House.LocationNotFound", (string)(string)"The specified location does not exist.", ErrorType.NotFound);
    public static readonly Error AmenitiesNotFound = 
        new((string)(string)"House.AmenitiesNotFound", (string)(string)"One or more of the specified amenities do not exist.", ErrorType.NotFound);
    public static readonly Error CreationError = 
        new((string)(string)"House.CreationError", (string)(string)"Could not retrieve the house details after creation.", ErrorType.NotFound);
    public static readonly Error HouseNotFound = 
        new((string)(string)"House.NotFound", (string)(string)"The house with the specified ID was not found.", ErrorType.NotFound);
    public static readonly Error NotOwner = 
        new((string)(string)"House.NotOwner", (string)(string)"You are not authorized to perform this action on the specified house.", ErrorType.Forbidden);
    public static readonly Error InsuffecientNumberOfBeds = 
        new((string)(string)"House.InsuffecientNumberOfBeds", (string)(string)"Insuffecient number of beds", ErrorType.BadRequest);
}

