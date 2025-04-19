using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.Section.Query;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Section;

public class GetSectionsByCourseIdQueryHandler : IQueryHandler<GetSectionsByCourseIdQuery, PagedResult<SectionResponse>>
{
    private readonly ILogger<GetSectionsByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;

    public GetSectionsByCourseIdQueryHandler(
        ILogger<GetSectionsByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _sectionLessonRepository = sectionLessonRepository;
    }

    public async Task<Result<PagedResult<SectionResponse>>> Handle(GetSectionsByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<PagedResult<SectionResponse>>(
                    new Error("Course.NotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Build the query
            IQueryable<Domain.Entities.Section> query = _sectionRepository.FindAll(s =>
                s.CourseId == request.CourseId && !s.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else
            {
                // Default sorting by creation date
                query = query.OrderBy(s => s.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Prepare section responses with additional data
            var sectionResponses = new List<SectionResponse>();
            foreach (var section in pagedItems)
            {
                // Count lessons in this section
                int lessonCount = _sectionLessonRepository.FindAll(sl =>
                    sl.SectionId == section.Id && !sl.IsDeleted).Count();

                sectionResponses.Add(new SectionResponse(
                    section.Id,
                    section.Name,
                    course.Id,
                    course.Name,
                    lessonCount,
                    section.CreatedAt,
                    section.UpdatedAt));
            }

            // Create paged result
            var pagedResult = PagedResult<SectionResponse>.Create(
                sectionResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting sections by course ID: {ex.Message}");
            return Result.Failure<PagedResult<SectionResponse>>(
                new Error("Section.QueryFailed", $"Failed to query sections: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.Section> ApplySorting(
        IQueryable<Domain.Entities.Section> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.Section, object>> keySelector = sortColumn.ToLower() switch
        {
            "name" => s => s.Name,
            "createdat" => s => s.CreatedAt,
            _ => s => s.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}