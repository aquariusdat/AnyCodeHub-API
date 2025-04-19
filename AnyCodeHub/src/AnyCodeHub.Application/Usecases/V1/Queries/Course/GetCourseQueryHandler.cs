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
using static AnyCodeHub.Contract.Services.V1.Course.Query;
using static AnyCodeHub.Contract.Services.V1.Course.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Course;

public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, PagedResult<CourseResponse>>
{
    private readonly ILogger<GetCourseQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Rating, Guid> _ratingRepository;
    private readonly IRepositoryBase<UserCourse, Guid> _userCourseRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public GetCourseQueryHandler(
        ILogger<GetCourseQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.Rating, Guid> ratingRepository,
        IRepositoryBase<UserCourse, Guid> userCourseRepository,
        UserManager<AppUser> userManager,
        IMapper mapper)
    {
        _logger = logger;
        _courseRepository = courseRepository;
        _sectionRepository = sectionRepository;
        _lessonRepository = lessonRepository;
        _ratingRepository = ratingRepository;
        _userCourseRepository = userCourseRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CourseResponse>>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Build the query
            IQueryable<Domain.Entities.Course> query = _courseRepository.FindAll(c => !c.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    (c.Description != null && c.Description.ToLower().Contains(searchTerm)));
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
                // Default sorting by creation date, newest first
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Prepare course responses with additional data
            var courseResponses = new List<CourseResponse>();
            foreach (var course in pagedItems)
            {
                // Get instructor details
                var instructor = await _userManager.FindByIdAsync(course.AuthorId.ToString());
                if (instructor == null) continue;

                // Calculate average rating
                var ratings = _ratingRepository.FindAll(r => r.CourseId == course.Id && !r.IsDeleted).ToList();
                double averageRating = ratings.Any() ? ratings.Average(r => r.Rate) : 0;

                // Count total students enrolled
                int totalStudents = _userCourseRepository.FindAll(uc => uc.CourseId == course.Id && !uc.IsDeleted).Count();

                // Count total sections and lessons
                int totalSections = _sectionRepository.FindAll(s => s.CourseId == course.Id && !s.IsDeleted).Count();
                int totalLessons = _lessonRepository.FindAll(l => l.CourseId == course.Id && !l.IsDeleted).Count();

                courseResponses.Add(new CourseResponse(
                    course.Id,
                    course.Name,
                    course.Description,
                    course.ImageUrl,
                    course.Price,
                    course.AuthorId,
                    instructor.UserName,
                    instructor.Avatar,
                    averageRating,
                    totalStudents,
                    totalLessons,
                    totalSections,
                    course.CreatedAt,
                    course.UpdatedAt));
            }

            // Create paged result
            var pagedResult = PagedResult<CourseResponse>.Create(
                courseResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting courses: {ex.Message}");
            return Result.Failure<PagedResult<CourseResponse>>(
                new Error("Course.QueryFailed", $"Failed to query courses: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.Course> ApplySorting(
        IQueryable<Domain.Entities.Course> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.Course, object>> keySelector = sortColumn.ToLower() switch
        {
            "name" => c => c.Name,
            "price" => c => c.Price,
            "createdat" => c => c.CreatedAt,
            "authorid" => c => c.AuthorId,
            _ => c => c.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}