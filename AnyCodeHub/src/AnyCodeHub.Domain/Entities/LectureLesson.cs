using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class LectureLesson : DomainEntity<Guid>, IBaseAuditEntity
{
    public Guid LectureId { get; private set; }
    public Guid LessonId { get; private set; }

    public DateTime CreatedAt { get;set; }
    public Guid CreatedBy { get;set; }
    public DateTime? UpdatedAt { get;set; }
    public Guid? UpdatedBy { get;set; }
    public bool IsDeleted { get;set; }
    public DateTime? DeletedAt { get;set; }
    public Guid? DeletedBy { get;set; }
}
