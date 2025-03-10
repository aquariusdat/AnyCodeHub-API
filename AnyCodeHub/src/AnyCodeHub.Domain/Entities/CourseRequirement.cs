using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseRequirement;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseRequirement : AggregateRoot<Guid>
{
    private CourseRequirement()
    {
    }

    private CourseRequirement(string requirementContent)
    {
        Id = Guid.NewGuid();
        RequirementContent = requirementContent;
    }

    public static CourseRequirement Create(string requirementContent)
    {
        CourseRequirement requirement = new CourseRequirement(requirementContent);
        requirement.RaiseDomainEvent(new DomainEvent.CourseRequirementCreated(Guid.NewGuid(), requirement.Id, requirement.RequirementContent));
        return requirement;
    }

    public void Update(Guid id, string requirementContent)
    {
        Id = id;
        RequirementContent = requirementContent;

        RaiseDomainEvent(new DomainEvent.CourseRequirementUpdated(Guid.NewGuid(), Id, RequirementContent));
    }

    public string RequirementContent { get; private set; }
}
