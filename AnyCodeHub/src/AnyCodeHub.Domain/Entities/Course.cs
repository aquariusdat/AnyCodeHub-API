using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Course : DomainEntity<Guid>, IBaseAuditEntity
{
    private Course()
    {
    }
    private Course(string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, int level, int totalViews, double totalDuration, double rating, Guid createdBy)
    {
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

    public static Course Create(string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, int level, int totalViews, double totalDuration, double rating, Guid createdBy)
        => new Course(name, description, price, salePrice, imageUrl, videoUrl, slug, status, authorId, level, totalViews, totalDuration, rating, createdBy);

    public void Update(Guid id, string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, int level, int totalViews, double totalDuration, double rating, Guid updatedBy)
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
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;
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
    public int Level { get; private set; }
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
