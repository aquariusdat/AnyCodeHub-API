using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.CourseCategory;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;
public class CourseCategory : AggregateRoot<Guid>, IBaseAuditEntity
{
    private CourseCategory()
    {
    }

    private CourseCategory(Guid courseId, Guid categoryId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        CategoryId = categoryId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static CourseCategory Create(Guid courseId, Guid categoryId, Guid createdBy)
    {
        CourseCategory courseCategory = new CourseCategory(courseId, categoryId, createdBy);
        courseCategory.RaiseDomainEvent(new DomainEvent.CourseCategoryCreated(Guid.NewGuid(), courseCategory.Id, courseId, categoryId, createdBy, DateTime.Now));
        return courseCategory;
    }

    public void Update(Guid id, Guid courseId, Guid categoryId, Guid updatedBy)
    {
        Id = id;
        CourseId = courseId;
        CategoryId = categoryId;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.CourseCategoryUpdated(Guid.NewGuid(), Id, CourseId, CategoryId, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CourseCategoryDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public Guid CourseId { get; private set; }
    public Guid CategoryId { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
