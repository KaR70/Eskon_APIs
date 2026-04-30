using Eskon.Application.Contracts.House;

namespace Eskon.Application.Contracts.Home;

public record HomeResponse(
    List<HouseSummaryResponse> Apartments,
    List<HouseSummaryResponse> Hotels,
    List<HouseSummaryResponse> Villas
);