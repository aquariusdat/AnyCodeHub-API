using System.Linq.Expressions;
using AnyCodeHub.Domain.Abstractions.Entities;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyCodeHub.Persistence.Repository;
public class RepositoryDbContextBase<TContext, TEntity, TKey> : IRepositoryDbContextBase<TContext, TEntity, TKey>, IDisposable
        where TContext : DbContext
        where TEntity : DomainEntity<TKey>
{

    private readonly TContext _context;

    public RepositoryDbContextBase(TContext context)
        => _context = context;

    public void Dispose()
        => _context?.Dispose();

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                items = items.Include(includeProperty);
            }
        }

        if (predicate is not null)
        {
            items = items.Where(predicate);
        }

        return items;
    }

    public async Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

    public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);

    public void Add(TEntity entity)
        => _context.Add(entity);

    public void Remove(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

    public void RemoveMultiple(List<TEntity> entities)
        => _context.Set<TEntity>().RemoveRange(entities);

    public void Update(TEntity entity)
        => _context.Set<TEntity>().Update(entity);
}
