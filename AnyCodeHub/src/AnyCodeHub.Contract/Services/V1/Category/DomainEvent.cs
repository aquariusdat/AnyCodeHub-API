using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.Category;

public static class DomainEvent
{
    public record CategoryCreated(Guid EventId, Guid Id, string Name, string? Description, Guid CreatedBy, DateTime CreatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = CreatedAt;
    }

    public record CategoryUpdated(Guid EventId, Guid Id, string Name, string? Description, Guid? UpdatedBy, DateTime? UpdatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = UpdatedAt ?? DateTimeOffset.Now;
    }

    public record CategoryDeleted(Guid EventId, Guid Id, Guid? DeletedBy, DateTime? DeletedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DeletedAt ?? DateTimeOffset.Now;
    }
}