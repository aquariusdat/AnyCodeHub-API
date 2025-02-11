using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class UserCourse : DomainEntity<Guid>, IBaseAuditEntity
{
    private UserCourse()
    {
        
    }

    private UserCourse(Guid UserId, Guid CourseId, Guid CreatedBy)
    {
        Id = Guid.NewGuid();
        this.UserId = UserId;
        this.CourseId = CourseId;
        CreatedAt = DateTime.Now;
        this.CreatedBy = CreatedBy;
    }   

    public static void Create(Guid UserId, Guid CourseId, Guid CreatedBy) => new UserCourse(UserId, CourseId, CreatedBy);

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
