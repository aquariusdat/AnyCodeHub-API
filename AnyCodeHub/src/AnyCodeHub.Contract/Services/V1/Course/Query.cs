using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Course.Response;

namespace AnyCodeHub.Contract.Services.V1.Course;

public static class Query
{
    public record GetCourseQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseResponse>>;

    public record GetCourseByIdQuery(Guid Id) : IQuery<CourseDetailResponse>;

    public record GetCoursesByUserIdQuery(
        Guid UserId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseResponse>>;

    public record GetCoursesByInstructorIdQuery(
        Guid InstructorId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseResponse>>;

    public record GetCoursesByCategoryIdQuery(
        Guid CategoryId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseResponse>>;

    public record GetCoursesByTechnologyIdQuery(
        Guid TechnologyId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseResponse>>;
}
