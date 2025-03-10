using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Services.V1.Lesson;
using AnyCodeHub.Domain.Abstractions.Aggregates;
using AnyCodeHub.Domain.Abstractions.Entities;

namespace AnyCodeHub.Domain.Entities;

public class Lesson : AggregateRoot<Guid>, IBaseAuditEntity
{
    private Lesson()
    {
    }

    private Lesson(string title, string? description, string? videoUrl, string? pdfUrl, Guid sectionId, Guid? courseId, double duration, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        PdfUrl = pdfUrl;
        SectionId = sectionId;
        CourseId = courseId;
        Duration = duration;
        CreatedBy = createdBy;
        CreatedAt = DateTime.Now;
    }

    public static Lesson Create(string title, string? description, string? videoUrl, string? pdfUrl, Guid sectionId, Guid? courseId, double duration, Guid createdBy)
    {
        Lesson lesson = new Lesson(title, description, videoUrl, pdfUrl, sectionId, courseId, duration, createdBy);
        lesson.RaiseDomainEvent(new DomainEvent.LessonCreated(Guid.NewGuid(), lesson.Id, title, description, videoUrl, pdfUrl, sectionId, courseId, duration, createdBy, DateTime.Now));
        return lesson;
    }

    public void Update(Guid id, string title, string? description, string? videoUrl, string? pdfUrl, Guid sectionId, Guid? courseId, double duration, Guid updatedBy)
    {
        Id = id;
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        PdfUrl = pdfUrl;
        SectionId = sectionId;
        CourseId = courseId;
        Duration = duration;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;

        RaiseDomainEvent(new DomainEvent.LessonUpdated(Guid.NewGuid(), Id, Title, Description, VideoUrl, PdfUrl, SectionId, CourseId, Duration, UpdatedBy, UpdatedAt));
    }

    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;

        RaiseDomainEvent(new DomainEvent.LessonDeleted(Guid.NewGuid(), Id, DeletedBy, DeletedAt));
    }

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