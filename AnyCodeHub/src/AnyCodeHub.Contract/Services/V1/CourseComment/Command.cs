using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseComment;

public static class Command
{
    public record CreateCourseCommentCommand(Guid courseId, Guid commentId, Guid createdBy) : ICommand<Response.CourseCommentResponse> { }
    public record UpdateCourseCommentCommand(Guid id, Guid courseId, Guid commentId, Guid updatedBy) : ICommand<Response.CourseCommentResponse> { }
    public record DeleteCourseCommentCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}