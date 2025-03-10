using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.Technology;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Technology : AggregateRoot<Guid>
{
    private Technology()
    {
    }

    private Technology(string name, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public static Technology Create(string name, string? description)
    {
        Technology technology = new Technology(name, description);
        technology.RaiseDomainEvent(new DomainEvent.TechnologyCreated(Guid.NewGuid(), technology.Id, name, description));
        return technology;
    }

    public void Update(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;

        RaiseDomainEvent(new DomainEvent.TechnologyUpdated(Guid.NewGuid(), Id, Name, Description));
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
}
