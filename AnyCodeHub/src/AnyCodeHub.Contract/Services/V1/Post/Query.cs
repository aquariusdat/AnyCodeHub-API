using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Post.Response;

namespace AnyCodeHub.Contract.Services.V1.Post;

public static class Query
{
    public record GetPostQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<PostResponse>>;

    public record GetPostByIdQuery(Guid Id) : IQuery<PostDetailResponse>;

    public record GetPostsByAuthorIdQuery(
        Guid AuthorId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<PostResponse>>;

    public record GetPostsByCategoryIdQuery(
        Guid CategoryId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<PostResponse>>;
}