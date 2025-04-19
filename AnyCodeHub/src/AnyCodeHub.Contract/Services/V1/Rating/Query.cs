using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Rating.Response;

namespace AnyCodeHub.Contract.Services.V1.Rating;

public static class Query
{
    public record GetRatingQuery(
        string? SearchTerm,
        string? SortColumn,
        SortOrder? SortOrder,
        Dictionary<string, SortOrder>? SortColumnAndOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<RatingResponse>>;

    public record GetRatingByIdQuery(Guid Id) : IQuery<RatingResponse>;

    public record GetRatingsByCourseIdQuery(
        Guid CourseId,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<RatingResponse>>;

    public record GetRatingsByUserIdQuery(
        Guid UserId,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<RatingResponse>>;

    public record GetCourseAverageRatingQuery(Guid CourseId) : IQuery<double>;
}