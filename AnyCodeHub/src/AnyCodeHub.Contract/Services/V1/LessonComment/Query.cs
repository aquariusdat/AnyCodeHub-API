using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Response;

namespace AnyCodeHub.Contract.Services.V1.LessonComment;

public static class Query
{
    public record GetLessonCommentQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<LessonCommentResponse>>;

    public record GetLessonCommentByIdQuery(Guid Id) : IQuery<LessonCommentResponse>;

    public record GetLessonCommentsByLessonIdQuery(
        Guid LessonId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<LessonCommentResponse>>;

    public record GetLessonCommentsByUserIdQuery(
        Guid UserId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<LessonCommentResponse>>;
}