using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Rating;

public static class DomainEvent
{
    public record RatingCreated(Guid EventId, Guid Id, Guid CourseId, Guid UserId, int Rate, Guid CreatedBy, DateTime CreatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = CreatedAt;
    }

    public record RatingUpdated(Guid EventId, Guid Id, Guid CourseId, Guid UserId, int Rate, Guid? UpdatedBy, DateTime? UpdatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = UpdatedAt ?? DateTimeOffset.Now;
    }

    public record RatingDeleted(Guid EventId, Guid Id, Guid? DeletedBy, DateTime? DeletedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DeletedAt ?? DateTimeOffset.Now;
    }
}