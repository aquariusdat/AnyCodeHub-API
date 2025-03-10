using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.CourseCategory;

public static class DomainEvent
{
    public record CourseCategoryCreated(Guid EventId, Guid Id, Guid CourseId, Guid CategoryId, Guid CreatedBy, DateTime CreatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = CreatedAt;
    }

    public record CourseCategoryUpdated(Guid EventId, Guid Id, Guid CourseId, Guid CategoryId, Guid? UpdatedBy, DateTime? UpdatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = UpdatedAt ?? DateTimeOffset.Now;
    }

    public record CourseCategoryDeleted(Guid EventId, Guid Id, Guid? DeletedBy, DateTime? DeletedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DeletedAt ?? DateTimeOffset.Now;
    }
}