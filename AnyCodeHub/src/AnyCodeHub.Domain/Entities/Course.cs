using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Contract.Services.V1.Course;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Course : AggregateRoot<Guid>, IBaseAuditEntity
{
    private Course()
    {
    }

    private Course(string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, CourseLevel level, int totalViews, double totalDuration, double rating, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        SalePrice = salePrice;
        ImageUrl = imageUrl;
        VideoUrl = videoUrl;
        Slug = slug;
        Status = status;
        AuthorId = authorId;
        Level = level;
        TotalViews = totalViews;
        TotalDuration = totalDuration;
        Rating = rating;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static Course Create(string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, CourseLevel level, int totalViews, double totalDuration, double rating, Guid createdBy)
    {
        Course course = new Course(name, description, price, salePrice, imageUrl, videoUrl, slug, status, authorId, level, totalViews, totalDuration, rating, createdBy);
        course.RaiseDomainEvent(new DomainEvent.CourseCreated(Guid.NewGuid(), course.Id, name, description, price, salePrice, imageUrl, videoUrl, slug, status, authorId, level, totalViews, totalDuration, rating, createdBy, DateTime.Now));
        return course;
    }

    public void Update(Guid id, string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, CourseLevel level, int totalViews, double totalDuration, double rating, Guid updatedBy)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        SalePrice = salePrice;
        ImageUrl = imageUrl;
        VideoUrl = videoUrl;
        Slug = slug;
        Status = status;
        AuthorId = authorId;
        Level = level;
        TotalViews = totalViews;
        TotalDuration = totalDuration;
        Rating = rating;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.CourseUpdated(Guid.NewGuid(), Id, name, description, price, salePrice, imageUrl, videoUrl, slug, status, authorId, level, totalViews, totalDuration, rating, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.CourseDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public decimal? SalePrice { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? VideoUrl { get; private set; }
    public string? Slug { get; private set; }
    public string Status { get; private set; }
    public Guid AuthorId { get; private set; }
    public CourseLevel Level { get; private set; }
    public int TotalViews { get; private set; }
    public double TotalDuration { get; private set; }
    public double Rating { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
