using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.Section;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Section : AggregateRoot<Guid>, IBaseAuditEntity
{
    private Section()
    {
    }

    private Section(string name, Guid courseId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        CourseId = courseId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static Section Create(string name, Guid courseId, Guid createdBy)
    {
        Section section = new Section(name, courseId, createdBy);
        section.RaiseDomainEvent(new DomainEvent.SectionCreated(Guid.NewGuid(), section.Id, name, courseId, createdBy, DateTime.Now));
        return section;
    }

    public void Update(Guid id, string name, Guid courseId, Guid updatedBy)
    {
        Id = id;
        Name = name;
        CourseId = courseId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.SectionUpdated(Guid.NewGuid(), Id, Name, CourseId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.SectionDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public string Name { get; private set; }
    public Guid CourseId { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
