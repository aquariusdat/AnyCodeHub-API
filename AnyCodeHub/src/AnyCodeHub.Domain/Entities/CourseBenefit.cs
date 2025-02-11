using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseBenefit : DomainEntity<Guid>
{
    public string BenefitContent { get; private set; }
}
