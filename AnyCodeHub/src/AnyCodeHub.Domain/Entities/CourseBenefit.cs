using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseBenefit;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseBenefit : AggregateRoot<Guid>, IBaseAuditEntity
{
    private CourseBenefit()
    {
    }

    private CourseBenefit(string benefitContent, Guid courseId, string description, Guid createdBy)
    {
        Id = Guid.NewGuid();
        BenefitContent = benefitContent;
        CourseId = courseId;
        Description = description;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public static CourseBenefit Create(string benefitContent, Guid courseId, string description, Guid createdBy)
    {
        CourseBenefit benefit = new CourseBenefit(benefitContent, courseId, description, createdBy);
        benefit.RaiseDomainEvent(new DomainEvent.CourseBenefitCreated(Guid.NewGuid(), benefit.Id, benefitContent));
        return benefit;
    }

    public void Update(Guid id, string benefitContent)
    {
        Id = id;
        BenefitContent = benefitContent;

        RaiseDomainEvent(new DomainEvent.CourseBenefitUpdated(Guid.NewGuid(), Id, benefitContent));
    }

    public string BenefitContent { get; private set; }
    public Guid CourseId { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
