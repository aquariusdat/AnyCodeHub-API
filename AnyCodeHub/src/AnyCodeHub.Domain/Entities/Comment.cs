using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.Comment;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Comment : AggregateRoot<Guid>, IBaseAuditEntity
{
    private Comment()
    {
    }

    private Comment(string content, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Content = content;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static Comment Create(string content, Guid createdBy)
    {
        Comment comment = new Comment(content, createdBy);
        comment.RaiseDomainEvent(new DomainEvent.CommentCreated(Guid.NewGuid(), comment.Id, content, createdBy, DateTime.Now));
        return comment;
    }

    public void Update(Guid id, string content, Guid updatedBy)
    {
        Id = id;
        Content = content;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.CommentUpdated(Guid.NewGuid(), Id, content, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CommentDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public string Content { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
