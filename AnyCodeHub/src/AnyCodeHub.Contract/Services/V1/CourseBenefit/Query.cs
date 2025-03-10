using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Response;

namespace AnyCodeHub.Contract.Services.V1.CourseBenefit;

public static class Query
{
    public record GetCourseBenefitQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<CourseBenefitResponse>>;
    public record GetCourseBenefitByIdQuery(Guid Id) : IQuery<CourseBenefitResponse>;
}