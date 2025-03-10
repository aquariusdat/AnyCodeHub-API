using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseRequirement;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseRequirement : AggregateRoot<Guid>, IBaseAuditEntity
{
    private CourseRequirement()
    {
    }

    private CourseRequirement(string requirementContent, Guid courseId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        RequirementContent = requirementContent;
        CourseId = courseId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public static CourseRequirement Create(string requirementContent, Guid courseId, Guid createdBy)
    {
        CourseRequirement requirement = new CourseRequirement(requirementContent, courseId, createdBy);
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
    public Guid CourseId { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

}
