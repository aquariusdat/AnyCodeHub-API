using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseComment.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseComment;

public static class Query
{
    public record GetCourseCommentQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<CourseCommentResponse>>;
    public record GetCourseCommentByIdQuery(Guid Id) : IQuery<CourseCommentResponse>;
}