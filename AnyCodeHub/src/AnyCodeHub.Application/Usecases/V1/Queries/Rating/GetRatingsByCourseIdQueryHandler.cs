using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.Rating.Query;
using static AnyCodeHub.Contract.Services.V1.Rating.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Rating;

public class GetRatingsByCourseIdQueryHandler : IQueryHandler<GetRatingsByCourseIdQuery, PagedResult<RatingResponse>>
{
    private readonly ILogger<GetRatingsByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Rating, Guid> _ratingRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public GetRatingsByCourseIdQueryHandler(
        ILogger<GetRatingsByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Rating, Guid> ratingRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        UserManager<AppUser> userManager,
        IMapper mapper)
    {
        _logger = logger;
        _ratingRepository = ratingRepository;
        _courseRepository = courseRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<RatingResponse>>> Handle(GetRatingsByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<PagedResult<RatingResponse>>(
                    new Error("Rating.CourseNotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Build the query
            IQueryable<Domain.Entities.Rating> query = _ratingRepository.FindAll(r =>
                r.CourseId == request.CourseId && !r.IsDeleted);

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else
            {
                // Default sorting by creation date (newest first)
                query = query.OrderByDescending(r => r.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Create the response DTOs with user details
            var ratingResponses = new List<RatingResponse>();
            foreach (var rating in pagedItems)
            {
                var user = await _userManager.FindByIdAsync(rating.UserId.ToString());
                if (user != null)
                {
                    ratingResponses.Add(new RatingResponse(
                        rating.Id,
                        rating.CourseId,
                        course.Name,
                        rating.UserId,
                        user.UserName,
                        rating.Rate,
                        rating.CreatedAt,
                        rating.UpdatedAt));
                }
            }

            // Create paged result
            var pagedResult = PagedResult<RatingResponse>.Create(
                ratingResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting ratings by course ID: {ex.Message}");
            return Result.Failure<PagedResult<RatingResponse>>(
                new Error("Rating.QueryFailed", $"Failed to query ratings: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.Rating> ApplySorting(
        IQueryable<Domain.Entities.Rating> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.Rating, object>> keySelector = sortColumn.ToLower() switch
        {
            "rate" => r => r.Rate,
            "createdat" => r => r.CreatedAt,
            "userid" => r => r.UserId,
            _ => r => r.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}