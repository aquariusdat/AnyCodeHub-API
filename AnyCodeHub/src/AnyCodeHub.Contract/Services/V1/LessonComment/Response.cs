namespace AnyCodeHub.Contract.Services.V1.LessonComment;

public static class Response
{
    public record LessonCommentResponse(Guid id, Guid lessonId, Guid commentId) { }
}