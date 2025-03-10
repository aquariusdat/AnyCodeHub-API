using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.LessonComment;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class LessonComment : AggregateRoot<Guid>, IBaseAuditEntity
{
    private LessonComment()
    {
    }

    private LessonComment(Guid lessonId, Guid commentId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        LessonId = lessonId;
        CommentId = commentId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static LessonComment Create(Guid lessonId, Guid commentId, Guid createdBy)
    {
        LessonComment lessonComment = new LessonComment(lessonId, commentId, createdBy);
        lessonComment.RaiseDomainEvent(new DomainEvent.LessonCommentCreated(Guid.NewGuid(), lessonComment.Id, lessonId, commentId, createdBy, DateTime.Now));
        return lessonComment;
    }

    public void Update(Guid id, Guid lessonId, Guid commentId, Guid updatedBy)
    {
        Id = id;
        LessonId = lessonId;
        CommentId = commentId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.LessonCommentUpdated(Guid.NewGuid(), Id, LessonId, CommentId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.LessonCommentDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid LessonId { get; private set; }
    public Guid CommentId { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
