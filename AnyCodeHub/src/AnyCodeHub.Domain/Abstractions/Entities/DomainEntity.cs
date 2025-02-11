namespace AnyCodeHub.Domain.Abstractions.Entities;

public abstract class DomainEntity<TKey> : IDomainEntity<TKey>
{
    public virtual TKey Id { get; protected set; }
}
