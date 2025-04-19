using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.Lesson.Query;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Lesson;

public class GetLessonQueryHandler : IQueryHandler<GetLessonQuery, PagedResult<LessonResponse>>
{
    private readonly ILogger<GetLessonQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IMapper _mapper;

    public GetLessonQueryHandler(
        ILogger<GetLessonQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<LessonResponse>>> Handle(GetLessonQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Build the query
            IQueryable<Domain.Entities.Lesson> query = _lessonRepository.FindAll(l => !l.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(l =>
                    l.Title.ToLower().Contains(searchTerm) ||
                    (l.Description != null && l.Description.ToLower().Contains(searchTerm)));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else if (request.SortColumnAndOrder != null && request.SortColumnAndOrder.Any())
            {
                foreach (var sort in request.SortColumnAndOrder)
                {
                    query = ApplySorting(query, sort.Key, sort.Value);
                }
            }
            else
            {
                // Default sorting by title
                query = query.OrderBy(l => l.Title);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var lessonResponses = _mapper.Map<List<LessonResponse>>(pagedItems);

            // Create paged result
            var pagedResult = PagedResult<LessonResponse>.Create(
                lessonResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting lessons: {ex.Message}");
            return Result.Failure<PagedResult<LessonResponse>>(
                new Error("Lesson.QueryFailed", $"Failed to query lessons: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.Lesson> ApplySorting(
        IQueryable<Domain.Entities.Lesson> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.Lesson, object>> keySelector = sortColumn.ToLower() switch
        {
            "title" => l => l.Title,
            "sectionid" => l => l.SectionId,
            "courseid" => l => l.CourseId,
            "duration" => l => l.Duration,
            "createdat" => l => l.CreatedAt,
            _ => l => l.Title // Default sort by title
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}