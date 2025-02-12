using AnyCodeHub.Domain.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AnyCodeHub.Persistence.Interceptors;
public class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IBaseAuditEntity>> entries = dbContext.ChangeTracker.Entries<IBaseAuditEntity>();

        foreach (EntityEntry<IBaseAuditEntity> entry in entries)
        {
            if (entry.State == EntityState.Added && entry.Property(t => t.CreatedAt).CurrentValue != DateTime.MinValue)
            {
                entry.Property(t => t.CreatedAt).CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified && !entry.Property(t => t.UpdatedAt).CurrentValue.HasValue)
            {
                entry.Property(t => t.UpdatedAt).CurrentValue = DateTime.Now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
