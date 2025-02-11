using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseQA : DomainEntity<Guid>
{
    public string Question { get; private set; }
    public string Answer { get; private set; }
}
