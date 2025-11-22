namespace Eskon_APIs.Contracts.Amenity;

/// <summary>
/// Request model for creating or updating an amenity.
/// </summary>
public class AmenityRequest
{
    /// <summary>
    /// Gets or sets the name of the amenity.
    /// </summary>
    /// <remarks>
    /// Examples: "WiFi", "Parking", "Swimming Pool", "Air Conditioning", "Garden"
    /// </remarks>
    public string AmenityName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the category of the amenity.
    /// </summary>
    /// <remarks>
    /// Examples: "Connectivity", "Parking", "Recreation", "Climate Control", "Outdoor"
    /// This field is optional.
    /// </remarks>
    public string? Category { get; set; }
}
