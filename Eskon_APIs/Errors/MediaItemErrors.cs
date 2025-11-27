namespace Eskon_APIs.Errors;

public static class MediaItemErrors
{
    public static readonly Error NoFile = new("Media.NoFile", "No file was uploaded.", StatusCodes.Status400BadRequest);
    public static readonly Error FileTooLarge = new("Media.FileTooLarge", "File size cannot exceed 5MB.", StatusCodes.Status400BadRequest);
    public static readonly Error NotFound = new("Media.NotFound", "The specified image was not found for this house.", StatusCodes.Status404NotFound);


}
