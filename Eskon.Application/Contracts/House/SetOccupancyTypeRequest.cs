namespace Eskon.Application.Contracts.House;

public record SetOccupancyTypeRequest(
    bool isShared,
    int BedCount
);
