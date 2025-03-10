using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.Category;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Category : AggregateRoot<Guid>, IBaseAuditEntity
{
    private Category()
    {
    }

    private Category(string name, string? description, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static Category Create(string name, string? description, Guid createdBy)
    {
        Category category = new Category(name, description, createdBy);
        category.RaiseDomainEvent(new DomainEvent.CategoryCreated(Guid.NewGuid(), category.Id, name, description, createdBy, DateTime.Now));
        return category;
    }

    public void Update(Guid id, string name, string? description, Guid updatedBy)
    {
        Id = id;
        Name = name;
        Description = description;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.CategoryUpdated(Guid.NewGuid(), Id, name, description, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CategoryDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
