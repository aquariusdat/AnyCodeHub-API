using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.UserCourse;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class UserCourse : AggregateRoot<Guid>, IBaseAuditEntity
{
    private UserCourse()
    {
    }

    private UserCourse(Guid userId, Guid courseId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CourseId = courseId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static UserCourse Create(Guid userId, Guid courseId, Guid createdBy)
    {
        UserCourse userCourse = new UserCourse(userId, courseId, createdBy);
        userCourse.RaiseDomainEvent(new DomainEvent.UserCourseCreated(Guid.NewGuid(), userCourse.Id, userId, courseId, createdBy, DateTime.Now));
        return userCourse;
    }

    public void Update(Guid id, Guid userId, Guid courseId, Guid updatedBy)
    {
        Id = id;
        UserId = userId;
        CourseId = courseId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.UserCourseUpdated(Guid.NewGuid(), Id, UserId, CourseId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.UserCourseDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
