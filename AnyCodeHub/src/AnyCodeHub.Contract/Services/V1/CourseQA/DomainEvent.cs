using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.CourseQA;

public static class DomainEvent
{
    public record CourseQACreated(Guid EventId, Guid Id, string Question, string Answer) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }

    public record CourseQAUpdated(Guid EventId, Guid Id, string Question, string Answer) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }
}