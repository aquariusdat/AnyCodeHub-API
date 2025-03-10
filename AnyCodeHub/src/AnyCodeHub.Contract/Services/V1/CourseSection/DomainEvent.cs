using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.CourseSection;

public static class DomainEvent
{
    public record CourseSectionCreated(Guid EventId, Guid Id, Guid CourseId, Guid LectureId, Guid CreatedBy, DateTime CreatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = CreatedAt;
    }

    public record CourseSectionUpdated(Guid EventId, Guid Id, Guid CourseId, Guid LectureId, Guid? UpdatedBy, DateTime? UpdatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = UpdatedAt ?? DateTimeOffset.Now;
    }

    public record CourseSectionDeleted(Guid EventId, Guid Id, Guid? DeletedBy, DateTime? DeletedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DeletedAt ?? DateTimeOffset.Now;
    }
}