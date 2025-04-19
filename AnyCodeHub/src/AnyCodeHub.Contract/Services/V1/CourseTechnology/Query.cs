using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseTechnology.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseTechnology;

public static class Query
{
    public record GetCourseTechnologyQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseTechnologyResponse>>;

    public record GetCourseTechnologyByIdQuery(Guid Id) : IQuery<CourseTechnologyResponse>;

    public record GetCourseTechnologiesByCourseIdQuery(
        Guid CourseId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseTechnologyResponse>>;

    public record GetCourseTechnologiesByTechnologyIdQuery(
        Guid TechnologyId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseTechnologyResponse>>;
}