using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Query;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseBenefit;

public class GetCourseBenefitsByCourseIdQueryHandler : IQueryHandler<GetCourseBenefitsByCourseIdQuery, PagedResult<CourseBenefitResponse>>
{
    private readonly ILogger<GetCourseBenefitsByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseBenefit, Guid> _courseBenefitRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseBenefitsByCourseIdQueryHandler(
        ILogger<GetCourseBenefitsByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseBenefit, Guid> courseBenefitRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseBenefitRepository = courseBenefitRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CourseBenefitResponse>>> Handle(GetCourseBenefitsByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<PagedResult<CourseBenefitResponse>>(
                    new Error("CourseBenefit.CourseNotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Build the query
            IQueryable<Domain.Entities.CourseBenefit> query = _courseBenefitRepository.FindAll(cb =>
                cb.CourseId == request.CourseId && !cb.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(cb =>
                    cb.BenefitContent.ToLower().Contains(searchTerm) ||
                    (cb.Description != null && cb.Description.ToLower().Contains(searchTerm)));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else
            {
                // Default sorting by creation date
                query = query.OrderBy(cb => cb.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var benefitResponses = pagedItems.Select(cb => new CourseBenefitResponse(
                cb.Id,
                cb.BenefitContent,
                cb.CourseId,
                cb.Description,
                cb.CreatedAt)).ToList();

            // Create paged result
            var pagedResult = PagedResult<CourseBenefitResponse>.Create(
                benefitResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course benefits: {ex.Message}");
            return Result.Failure<PagedResult<CourseBenefitResponse>>(
                new Error("CourseBenefit.QueryFailed", $"Failed to query course benefits: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.CourseBenefit> ApplySorting(
        IQueryable<Domain.Entities.CourseBenefit> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.CourseBenefit, object>> keySelector = sortColumn.ToLower() switch
        {
            "benefitcontent" => cb => cb.BenefitContent,
            "description" => cb => cb.Description,
            "createdat" => cb => cb.CreatedAt,
            _ => cb => cb.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}