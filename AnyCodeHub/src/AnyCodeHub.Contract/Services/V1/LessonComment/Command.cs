using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.LessonComment;

public static class Command
{
    public record CreateLessonCommentCommand(Guid lessonId, Guid commentId, string content, Guid createdBy) : ICommand<Response.LessonCommentResponse> { }
    public record UpdateLessonCommentCommand(Guid id, Guid lessonId, Guid commentId, string content, Guid updatedBy) : ICommand<Response.LessonCommentResponse> { }
    public record DeleteLessonCommentCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}