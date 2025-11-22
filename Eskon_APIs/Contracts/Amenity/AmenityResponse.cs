namespace Eskon_APIs.Contracts.Amenity;

public class AmenityResponse
{
    public int AmenityId { get; set; }

    public string AmenityName { get; set; } = null!;
    public string? Category { get; set; }
}
