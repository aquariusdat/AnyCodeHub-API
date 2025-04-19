using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Category.Response;

namespace AnyCodeHub.Contract.Services.V1.Category;

public static class Query
{
    public record GetCategoryQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<CategoryResponse>>;

    public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResponse>;

    public record GetCategoriesByIdsQuery(List<Guid> Ids) : IQuery<IEnumerable<CategoryResponse>>;
}