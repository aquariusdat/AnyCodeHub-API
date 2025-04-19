using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseCategory;

public static class Query
{
    public record GetCourseCategoryQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseCategoryResponse>>;

    public record GetCourseCategoryByIdQuery(Guid Id) : IQuery<CourseCategoryDetailResponse>;

    public record GetCourseCategoriesByCourseIdQuery(
        Guid CourseId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseCategoryResponse>>;

    public record GetCourseCategoryByCourseIdQuery(Guid CourseId) : IQuery<List<CourseCategoryResponse>>;

    public record GetCourseCategoriesByCategoryIdQuery(
        Guid CategoryId,
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CourseCategoryResponse>>;
}