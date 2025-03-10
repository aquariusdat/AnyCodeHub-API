using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.Comment;

public static class Command
{
    public record CreateCommentCommand(string content, Guid createdBy) : ICommand<Response.CommentResponse> { }
    public record UpdateCommentCommand(Guid id, string content, Guid updatedBy) : ICommand<Response.CommentResponse> { }
    public record DeleteCommentCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}