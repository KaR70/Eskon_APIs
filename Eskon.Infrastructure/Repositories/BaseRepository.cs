using System.Linq.Expressions;
using Eskon.Domain.Interfaces;
using Eskon.Infrastructure.Persistance;

namespace Eskon.Infrastructure.Repositories;

public class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context = context;

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await Context.Set<T>().FindAsync(id, cancellationToken);
    

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) 
        => await Context.Set<T>().ToListAsync(cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
        => await Context.Set<T>().AnyAsync(criteria, cancellationToken);
    
    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        => await Context.Set<T>().AnyAsync(cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
        => await Context.Set<T>().FirstOrDefaultAsync(criteria, cancellationToken);
    
    public Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
        => Context.Set<T>().CountAsync(criteria, cancellationToken);
    
    public Task<int> CountAsync(CancellationToken cancellationToken = default) => Context.Set<T>().CountAsync(cancellationToken);

    public void Update(T entity) => Context.Set<T>().Update(entity);
    
    public void Delete(T entity) => Context.Set<T>().Remove(entity);
    
    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }
    
    
}