using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Technology;

public static class DomainEvent
{
    public record TechnologyCreated(Guid EventId, Guid Id, string Name, string? Description) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }

    public record TechnologyUpdated(Guid EventId, Guid Id, string Name, string? Description) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }
}