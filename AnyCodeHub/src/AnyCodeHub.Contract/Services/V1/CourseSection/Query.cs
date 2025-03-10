using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseSection.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseSection;

public static class Query
{
    public record GetCourseSectionQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<CourseSectionResponse>>;
    public record GetCourseSectionByIdQuery(Guid Id) : IQuery<CourseSectionResponse>;
}