using Eskon.Application.Contracts.Authentication;
using Eskon.Application.Contracts.House;
using Eskon.Domain.Entities;
using Mapster;

namespace Eskon.Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => $"{src.FirstName} {src.LastName}")
            //.Map(dest => dest.UserName, src => src.Email)
            ;

        config.NewConfig<House, HouseSummaryResponse>()
            .Map(dest => dest.CoverImageUrl,
                src => Enumerable
                    .Where<MediaItem>(src.MediaItems, m => m.IsCover)
                    .Select(m => m.URL)
                    .FirstOrDefault()
                  ?? Enumerable
                       .OrderBy<MediaItem, int>(src.MediaItems, m => m.SortOrder)
                       .Select(m => m.URL)
                       .FirstOrDefault()
                  ?? string.Empty)
            .Map(dest => dest.FormattedLocation,
                src => string.Join(", ", Enumerable.Where<string>(new[] { src.Location.City, src.Location.Street }, x => !string.IsNullOrWhiteSpace(x))))
            .Map(dest => dest.OwnerId, src => src.OwnerId);


        config.NewConfig<House, HouseDetailResponse>()
            .Map(dest => dest.Amenities, src => Enumerable.Select<HouseAmenity, Amenity>(src.HouseAmenities, ha => ha.Amenity))
            .Map(dest => dest.ImageUrls, src => Enumerable.Select<MediaItem, string>(src.MediaItems, mi => mi.URL))
            .Map(dest => dest.Owner.FullName, src => $"{src.Owner.FirstName} {src.Owner.LastName}")
            .Map(dest => dest.Owner.UserId, src => src.OwnerId)
            .Map(dest => dest.Owner.Email, src => src.Owner.Email);
    }
}