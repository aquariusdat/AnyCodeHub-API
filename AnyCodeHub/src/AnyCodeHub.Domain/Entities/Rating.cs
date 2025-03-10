using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.Rating;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Rating : AggregateRoot<Guid>, IBaseAuditEntity
{
    private Rating()
    {
    }

    private Rating(Guid courseId, Guid userId, int rate, Guid createdBy)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        UserId = userId;
        Rate = rate;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static Rating Create(Guid courseId, Guid userId, int rate, Guid createdBy)
    {
        Rating rating = new Rating(courseId, userId, rate, createdBy);
        rating.RaiseDomainEvent(new DomainEvent.RatingCreated(Guid.NewGuid(), rating.Id, courseId, userId, rate, createdBy, DateTime.Now));
        return rating;
    }

    public void Update(Guid id, Guid courseId, Guid userId, int rate, Guid updatedBy)
    {
        Id = id;
        CourseId = courseId;
        UserId = userId;
        Rate = rate;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.RatingUpdated(Guid.NewGuid(), Id, CourseId, UserId, Rate, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.RatingDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid CourseId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rate { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
