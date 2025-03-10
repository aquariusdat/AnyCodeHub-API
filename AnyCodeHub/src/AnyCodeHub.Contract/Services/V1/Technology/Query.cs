using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Technology.Response;

namespace AnyCodeHub.Contract.Services.V1.Technology;

public static class Query
{
    public record GetTechnologyQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<TechnologyResponse>>;
    public record GetTechnologyByIdQuery(Guid Id) : IQuery<TechnologyResponse>;
}