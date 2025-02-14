using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using System.Windows.Input;

namespace AnyCodeHub.Contract.Services.V1.Course;

public static class Command
{
    public record CreateCourseCommand(string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, int level, int totalViews, double totalDuration, double rating, Guid createdBy) : ICommand<Result<Response.CourseResponse>> { }
    public record UpdateCourseCommand(Guid id, string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, int level, int totalViews, double totalDuration, double rating, Guid updatedBy) : ICommand<Result<Response.CourseResponse>> { }
    public record DeleteCourseCommand(Guid id, Guid deletedBy) : ICommand<bool> {}

}
