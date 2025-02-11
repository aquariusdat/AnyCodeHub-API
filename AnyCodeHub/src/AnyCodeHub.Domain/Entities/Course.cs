using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Course : DomainEntity<Guid>, IBaseAuditEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public decimal? SalePrice { get; private set; }
    public string Slug { get; private set; }
    public string Status { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Level { get; private set; }
    public int TotalViews { get; private set; }
    public double Rating { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
