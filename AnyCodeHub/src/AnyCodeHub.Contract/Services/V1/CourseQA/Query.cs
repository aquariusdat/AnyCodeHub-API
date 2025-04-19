using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseQA;

public static class Query
{
    public record GetCourseQAQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseQAResponse>>;

    public record GetCourseQAByIdQuery(Guid Id) : IQuery<CourseQAResponse>;

    public record GetCourseQAsByCourseIdQuery(
        Guid CourseId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseQAResponse>>;
}