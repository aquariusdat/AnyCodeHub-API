using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Lesson : DomainEntity<Guid>, IBaseAuditEntity
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public string? VideoUrl { get; private set; }
    public string? PdfUrl { get; private set; }
    public Guid SectionId { get; private set; }
    /// <summary>
    /// Tuy hơi thừa nhưng giúp truy vấn nhanh hơn
    /// </summary>
    public Guid? CourseId { get; private set; }
    public double Duration { get; private set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}