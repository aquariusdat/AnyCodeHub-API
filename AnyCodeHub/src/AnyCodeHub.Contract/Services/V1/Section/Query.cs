using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Contract.Services.V1.Section;

public static class Query
{
    public record GetSectionQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<SectionResponse>>;
    public record GetSectionByIdQuery(Guid Id) : IQuery<SectionResponse>;
}