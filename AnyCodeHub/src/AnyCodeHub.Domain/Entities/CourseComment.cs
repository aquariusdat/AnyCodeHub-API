using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseComment;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class CourseComment : AggregateRoot<Guid>, IBaseAuditEntity
{
    private CourseComment()
    {
    }

    private CourseComment(Guid courseId, Guid commentId, string content, Guid createdBy)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        CommentId = commentId;
        CreatedBy = createdBy;
        Content = content;
        CreatedAt = DateTime.Now;
    }

    public static CourseComment Create(Guid courseId, Guid commentId, string content, Guid createdBy)
    {
        CourseComment courseComment = new CourseComment(courseId, commentId, content, createdBy);
        courseComment.RaiseDomainEvent(new DomainEvent.CourseCommentCreated(Guid.NewGuid(), courseComment.Id, courseId, commentId, content, createdBy, DateTime.Now));
        return courseComment;
    }

    public void Update(Guid id, Guid courseId, Guid commentId, string content, Guid updatedBy)
    {
        Id = id;
        CourseId = courseId;
        CommentId = commentId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;
        Content = content;

        RaiseDomainEvent(new DomainEvent.CourseCommentUpdated(Guid.NewGuid(), Id, CourseId, CommentId, Content, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CourseCommentDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid CourseId { get; private set; }
    public Guid CommentId { get; private set; }
    public string Content { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
