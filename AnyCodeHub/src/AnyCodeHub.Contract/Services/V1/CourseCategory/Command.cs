using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseCategory;

public static class Command
{
    public record CreateCourseCategoryCommand(Guid courseId, Guid categoryId, Guid createdBy) : ICommand<Response.CourseCategoryResponse> { }
    public record UpdateCourseCategoryCommand(Guid id, Guid courseId, Guid categoryId, Guid updatedBy) : ICommand<Response.CourseCategoryResponse> { }
    public record DeleteCourseCategoryCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}