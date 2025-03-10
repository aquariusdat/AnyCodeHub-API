using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.CourseTechnology;

public static class DomainEvent
{
    public record CourseTechnologyCreated(Guid EventId, Guid Id, Guid CourseId, Guid TechnologyId) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }

    public record CourseTechnologyUpdated(Guid EventId, Guid Id, Guid CourseId, Guid TechnologyId) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }
}