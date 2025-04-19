using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Query;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseQA;

public class GetCourseQAsByCourseIdQueryHandler : IQueryHandler<GetCourseQAsByCourseIdQuery, PagedResult<CourseQAResponse>>
{
    private readonly ILogger<GetCourseQAsByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseQA, Guid> _courseQARepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseQAsByCourseIdQueryHandler(
        ILogger<GetCourseQAsByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseQA, Guid> courseQARepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseQARepository = courseQARepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CourseQAResponse>>> Handle(GetCourseQAsByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<PagedResult<CourseQAResponse>>(
                    new Error("CourseQA.CourseNotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Build the query
            IQueryable<Domain.Entities.CourseQA> query = _courseQARepository.FindAll(qa =>
                qa.CourseId == request.CourseId && !qa.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(qa =>
                    qa.Question.ToLower().Contains(searchTerm) ||
                    qa.Answer.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else
            {
                // Default sorting by creation date
                query = query.OrderBy(qa => qa.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var qaResponses = pagedItems.Select(qa => new CourseQAResponse(
                qa.Id,
                qa.Question,
                qa.Answer,
                qa.CourseId,
                qa.CreatedAt,
                qa.UpdatedAt)).ToList();

            // Create paged result
            var pagedResult = PagedResult<CourseQAResponse>.Create(
                qaResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course QAs: {ex.Message}");
            return Result.Failure<PagedResult<CourseQAResponse>>(
                new Error("CourseQA.QueryFailed", $"Failed to query course QAs: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.CourseQA> ApplySorting(
        IQueryable<Domain.Entities.CourseQA> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.CourseQA, object>> keySelector = sortColumn.ToLower() switch
        {
            "question" => qa => qa.Question,
            "answer" => qa => qa.Answer,
            "createdat" => qa => qa.CreatedAt,
            _ => qa => qa.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}