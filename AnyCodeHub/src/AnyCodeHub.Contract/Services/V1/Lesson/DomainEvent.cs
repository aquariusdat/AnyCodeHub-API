using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Lesson;

public static class DomainEvent
{
    public record LessonCreated(Guid EventId, Guid Id, string Title, string? Description, string? VideoUrl, string? PdfUrl, Guid SectionId, Guid? CourseId, double Duration, Guid CreatedBy, DateTime CreatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = CreatedAt;
    }

    public record LessonUpdated(Guid EventId, Guid Id, string Title, string? Description, string? VideoUrl, string? PdfUrl, Guid SectionId, Guid? CourseId, double Duration, Guid? UpdatedBy, DateTime? UpdatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = UpdatedAt ?? DateTimeOffset.Now;
    }

    public record LessonDeleted(Guid EventId, Guid Id, Guid? DeletedBy, DateTime? DeletedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DeletedAt ?? DateTimeOffset.Now;
    }
}