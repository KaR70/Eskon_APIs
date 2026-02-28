namespace Eskon.Domain.Errors;

public static class MediaItemErrors
{
    public static readonly Error NoFile = 
        new((string)(string)"Media.NoFile", (string)(string)"No file was uploaded.", ErrorType.BadRequest);
    public static readonly Error FileTooLarge = 
        new((string)(string)"Media.FileTooLarge", (string)(string)"File size cannot exceed 5MB.", ErrorType.BadRequest);
    public static readonly Error NotFound = 
        new((string)(string)"Media.NotFound", (string)(string)"The specified image was not found for this house.", ErrorType.NotFound);


}
