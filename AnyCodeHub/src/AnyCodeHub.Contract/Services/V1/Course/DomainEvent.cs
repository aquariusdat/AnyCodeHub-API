using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Enumerations;

namespace AnyCodeHub.Contract.Services.V1.Course;

public static class DomainEvent
{
    public record CourseCreated(Guid EventId, Guid Id, string Name, string? Description, decimal Price, decimal? SalePrice, string? ImageUrl, string? VideoUrl, string? Slug, string Status, Guid AuthorId, CourseLevel Level, int TotalViews, double TotalDuration, double Rating, Guid CreatedBy, DateTime CreatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = CreatedAt;
    }

    public record CourseUpdated(Guid EventId, Guid Id, string Name, string? Description, decimal Price, decimal? SalePrice, string? ImageUrl, string? VideoUrl, string? Slug, string Status, Guid AuthorId, CourseLevel Level, int TotalViews, double TotalDuration, double Rating, Guid? UpdatedBy, DateTime? UpdatedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = UpdatedAt ?? DateTimeOffset.Now;
    }

    public record CourseDeleted(Guid EventId, Guid Id, Guid? DeletedBy, DateTime? DeletedAt) : IDomainEvent
    {
        public Guid Id { get; set; } = Id;
        public DateTimeOffset TimeStamp { get; set; } = DeletedAt ?? DateTimeOffset.Now;
    }
}
