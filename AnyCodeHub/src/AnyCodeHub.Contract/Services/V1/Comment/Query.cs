using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Comment.Response;

namespace AnyCodeHub.Contract.Services.V1.Comment;

public static class Query
{
    public record GetCommentQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<CommentResponse>>;
    public record GetCommentByIdQuery(Guid Id) : IQuery<CommentResponse>;
}