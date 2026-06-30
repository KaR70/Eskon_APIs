using System.ComponentModel.DataAnnotations;

namespace Eskon.Application.Contracts.House;

public class CreateHouseRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal PricePerMonth { get; set; }

    [Range(0, 50)]
    public int NumberOfRooms { get; set; }

    [Range(1, 20)]
    public int NumberOfBathrooms { get; set; }

    [Range(1, 10000)]
    public double Area { get; set; }

    public bool IsShared { get; set; }
    public int? BedCount { get; set; }

    public int LocationId { get; set; }

    public List<int> AmenityIds { get; set; } = new();
}
