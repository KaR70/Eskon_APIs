using Eskon.Domain.Entities;
using Eskon.Domain.Interfaces;

namespace Eskon.Application;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<Amenity> Amenities { get; }
    IBaseRepository<Location> Locations { get; }
    ISavedListRepository SavedLists { get; }
    IHouseRepository Houses { get; }
    IMediaItemRepository MediaItems { get; }
    int Complete();
    Task<int> CompleteAsync();
}