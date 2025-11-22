using Eskon_APIs.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Eskon_APIs.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    public DbSet<House> House { get; set; }
    public DbSet<SavedList> SavedList { get; set; }
    public DbSet<Amenity> Amenity { get; set; }
    public DbSet<Location> Location { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}