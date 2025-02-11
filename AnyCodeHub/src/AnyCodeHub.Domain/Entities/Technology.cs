using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Technology : DomainEntity<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
