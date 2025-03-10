using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.CourseRequirement;

public static class DomainEvent
{
    public record CourseRequirementCreated(Guid EventId, Guid Id, string RequirementContent) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }

    public record CourseRequirementUpdated(Guid EventId, Guid Id, string RequirementContent) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }
}