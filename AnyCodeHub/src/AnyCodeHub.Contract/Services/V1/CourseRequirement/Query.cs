using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseRequirement;

public static class Query
{
    public record GetCourseRequirementQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<CourseRequirementResponse>>;
    public record GetCourseRequirementByIdQuery(Guid Id) : IQuery<CourseRequirementResponse>;
}