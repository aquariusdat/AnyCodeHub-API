using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseTechnology : DomainEntity<Guid>
{
    public Guid CourseId { get; private set; }
    public Guid TechnologyId { get; private set; }

}
