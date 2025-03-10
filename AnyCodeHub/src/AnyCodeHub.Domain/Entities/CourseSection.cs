using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseSection;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseSection : AggregateRoot<Guid>, IBaseAuditEntity
{
    private CourseSection()
    {
    }

    private CourseSection(string name, Guid courseId, Guid sectionId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        CourseId = courseId;
        SectionId = sectionId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static CourseSection Create(string name, Guid courseId, Guid sectionId, Guid createdBy)
    {
        CourseSection courseSection = new CourseSection(name, courseId, sectionId, createdBy);
        courseSection.RaiseDomainEvent(new DomainEvent.CourseSectionCreated(Guid.NewGuid(), courseSection.Id, courseId, sectionId, createdBy, DateTime.Now));
        return courseSection;
    }

    public void Update(Guid id, Guid courseId, Guid sectionId, Guid updatedBy)
    {
        Id = id;
        CourseId = courseId;
        SectionId = sectionId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.CourseSectionUpdated(Guid.NewGuid(), Id, CourseId, SectionId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CourseSectionDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid CourseId { get; private set; }
    public Guid SectionId { get; private set; }
    public string Name { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
