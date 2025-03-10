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

    private CourseSection(Guid courseId, Guid lectureId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        LectureId = lectureId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static CourseSection Create(Guid courseId, Guid lectureId, Guid createdBy)
    {
        CourseSection courseSection = new CourseSection(courseId, lectureId, createdBy);
        courseSection.RaiseDomainEvent(new DomainEvent.CourseSectionCreated(Guid.NewGuid(), courseSection.Id, courseId, lectureId, createdBy, DateTime.Now));
        return courseSection;
    }

    public void Update(Guid id, Guid courseId, Guid lectureId, Guid updatedBy)
    {
        Id = id;
        CourseId = courseId;
        LectureId = lectureId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.CourseSectionUpdated(Guid.NewGuid(), Id, CourseId, LectureId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CourseSectionDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid CourseId { get; private set; }
    public Guid LectureId { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
