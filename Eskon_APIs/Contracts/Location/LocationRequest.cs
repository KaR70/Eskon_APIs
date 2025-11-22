namespace Eskon_APIs.Contracts.Location;

public class LocationRequest
{
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? PostalCode { get; set; }
    public string Street { get; set; } = null!;
    public string BuildingNumber { get; set; } = null!;
    public string? GeoLat { get; set; }
    public string? GeoLng { get; set; }
}
