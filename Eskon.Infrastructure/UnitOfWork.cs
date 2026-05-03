using Eskon.Application;
using Eskon.Domain.Entities;
using Eskon.Domain.Interfaces;
using Eskon.Infrastructure.Persistance;
using Eskon.Infrastructure.Repositories;

namespace Eskon.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IBaseRepository<Amenity> Amenities { get; }
    public IBaseRepository<Location> Locations { get; }
    public IHouseRepository Houses { get; }
    public IMediaItemRepository MediaItems { get; }
    public ISavedListRepository SavedLists { get; }
    public IReviewRepository Reviews { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Amenities = new BaseRepository<Amenity>(context);
        Locations = new BaseRepository<Location>(context);
        Houses = new HouseRepository(context);
        MediaItems = new MediaItemRepository(context);
        SavedLists = new SavedListRepository(context);
        Reviews = new ReviewRepository(context);
    }

    public void Dispose() => _context.Dispose();

    public int Complete() => _context.SaveChanges();

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
}