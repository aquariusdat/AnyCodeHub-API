using AnyCodeHub.Domain.Abstractions;
using AnyCodeHub.Domain.Abstractions.Entities;
using AnyCodeHub.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace AnyCodeHub.Persistence;
public class EFDbContextUnitOfWork<TContext> : IUnitOfWorkDbContext<TContext>
    where TContext : DbContext
{
    private readonly TContext _context;

    public EFDbContextUnitOfWork(TContext context)
        => _context = context;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //ConvertDomainEventsToOutboxMessages();
        //UpdateAuditableEntities();
        await _context.SaveChangesAsync();
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IBaseAuditEntity>> entries = _context.ChangeTracker.Entries<IBaseAuditEntity>();

        foreach (EntityEntry<IBaseAuditEntity> entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(t => t.CreatedAt).CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(t => t.UpdatedAt).CurrentValue = DateTime.Now;
            }
        }
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = _context.ChangeTracker.Entries<Domain.Abstractions.Aggregates.AggregateRoot<Guid>>()
                                .Select(t => t.Entity)
                                .SelectMany(aggregateRoot =>
                                {
                                    var domainEvents = aggregateRoot.GetDomainEvents();

                                    aggregateRoot.ClearDomainEvents();

                                    return domainEvents;
                                })
                                .Select(domainEvent => new OutboxMessage
                                {
                                    Id = Guid.NewGuid(),
                                    OccurredOnUtc = DateTime.Now,
                                    Type = domainEvent.GetType().Name,
                                    Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All,
                                    }),
                                })
                                .ToList();

        _context.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
        => await _context.DisposeAsync();
}
