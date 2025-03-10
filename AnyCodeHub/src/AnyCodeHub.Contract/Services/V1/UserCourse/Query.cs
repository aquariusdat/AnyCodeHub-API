using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.UserCourse.Response;

namespace AnyCodeHub.Contract.Services.V1.UserCourse;

public static class Query
{
    public record GetUserCourseQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<UserCourseResponse>>;
    public record GetUserCourseByIdQuery(Guid Id) : IQuery<UserCourseResponse>;
}