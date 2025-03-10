using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.SectionLesson;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class SectionLesson : AggregateRoot<Guid>, IBaseAuditEntity
{
    private SectionLesson()
    {
    }

    private SectionLesson(Guid sectionId, Guid lessonId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        SectionId = sectionId;
        LessonId = lessonId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static SectionLesson Create(Guid sectionId, Guid lessonId, Guid createdBy)
    {
        SectionLesson sectionLesson = new SectionLesson(sectionId, lessonId, createdBy);
        sectionLesson.RaiseDomainEvent(new DomainEvent.SectionLessonCreated(Guid.NewGuid(), sectionLesson.Id, sectionId, lessonId, createdBy, DateTime.Now));
        return sectionLesson;
    }

    public void Update(Guid id, Guid sectionId, Guid lessonId, Guid updatedBy)
    {
        Id = id;
        SectionId = sectionId;
        LessonId = lessonId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.SectionLessonUpdated(Guid.NewGuid(), Id, SectionId, LessonId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.SectionLessonDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid SectionId { get; private set; }
    public Guid LessonId { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
