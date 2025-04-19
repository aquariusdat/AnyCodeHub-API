using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Query;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseRequirement;

public class GetCourseRequirementsByCourseIdQueryHandler : IQueryHandler<GetCourseRequirementsByCourseIdQuery, PagedResult<CourseRequirementResponse>>
{
    private readonly ILogger<GetCourseRequirementsByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseRequirement, Guid> _courseRequirementRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseRequirementsByCourseIdQueryHandler(
        ILogger<GetCourseRequirementsByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseRequirement, Guid> courseRequirementRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseRequirementRepository = courseRequirementRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CourseRequirementResponse>>> Handle(GetCourseRequirementsByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<PagedResult<CourseRequirementResponse>>(
                    new Error("CourseRequirement.CourseNotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Build the query
            IQueryable<Domain.Entities.CourseRequirement> query = _courseRequirementRepository.FindAll(cr =>
                cr.CourseId == request.CourseId && !cr.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(cr => cr.RequirementContent.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else
            {
                // Default sorting by creation date
                query = query.OrderBy(cr => cr.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var requirementResponses = pagedItems.Select(cr => new CourseRequirementResponse(
                cr.Id,
                cr.RequirementContent,
                cr.CourseId,
                cr.CreatedAt,
                cr.UpdatedAt)).ToList();

            // Create paged result
            var pagedResult = PagedResult<CourseRequirementResponse>.Create(
                requirementResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course requirements: {ex.Message}");
            return Result.Failure<PagedResult<CourseRequirementResponse>>(
                new Error("CourseRequirement.QueryFailed", $"Failed to query course requirements: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.CourseRequirement> ApplySorting(
        IQueryable<Domain.Entities.CourseRequirement> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.CourseRequirement, object>> keySelector = sortColumn.ToLower() switch
        {
            "requirementcontent" => cr => cr.RequirementContent,
            "createdat" => cr => cr.CreatedAt,
            _ => cr => cr.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}