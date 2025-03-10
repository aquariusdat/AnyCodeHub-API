namespace AnyCodeHub.Contract.Services.V1.CourseComment;

public static class Response
{
    public record CourseCommentResponse(Guid id, Guid courseId, Guid commentId) { }
}