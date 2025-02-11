using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseRequirement : DomainEntity<Guid>
{
    public string RequirementContent { get; private set; }
}
