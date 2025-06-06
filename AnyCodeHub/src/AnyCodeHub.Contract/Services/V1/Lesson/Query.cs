using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;

namespace AnyCodeHub.Contract.Services.V1.Lesson;

public static class Query
{
    public record GetLessonQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, Dictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<LessonResponse>>;
    public record GetLessonByIdQuery(Guid Id) : IQuery<LessonResponse>;
    public record GetLessonsBySectionIdQuery(Guid SectionId) : IQuery<IEnumerable<LessonResponse>>;
    public record GetLessonsByCourseIdQuery(Guid CourseId) : IQuery<IEnumerable<LessonResponse>>;
}