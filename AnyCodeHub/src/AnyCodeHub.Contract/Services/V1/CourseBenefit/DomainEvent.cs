using System;
using AnyCodeHub.Contract.Abstractions.Message;

namespace AnyCodeHub.Contract.Services.V1.CourseBenefit;

public static class DomainEvent
{
    public record CourseBenefitCreated(Guid EventId, Guid Id, string BenefitContent) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }

    public record CourseBenefitUpdated(Guid EventId, Guid Id, string BenefitContent) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }
}