using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Domain.Abstractions.Entities;
using System.Collections.Generic;

namespace AnyCodeHub.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot<T> : DomainEntity<T>
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _events.ToList();

    public void ClearDomainEvents() => _events.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);
}