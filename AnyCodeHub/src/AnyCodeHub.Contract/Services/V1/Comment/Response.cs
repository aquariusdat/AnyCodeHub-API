namespace AnyCodeHub.Contract.Services.V1.Comment;

public static class Response
{
    public record CommentResponse(Guid id, string content) { }
}