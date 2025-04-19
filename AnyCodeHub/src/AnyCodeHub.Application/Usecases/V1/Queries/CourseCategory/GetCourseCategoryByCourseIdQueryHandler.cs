using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Query;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseCategory;

public class GetCourseCategoryByCourseIdQueryHandler : IQueryHandler<GetCourseCategoryByCourseIdQuery, List<CourseCategoryResponse>>
{
    private readonly ILogger<GetCourseCategoryByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseCategory, Guid> _courseCategoryRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;

    public GetCourseCategoryByCourseIdQueryHandler(
        ILogger<GetCourseCategoryByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseCategory, Guid> courseCategoryRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    {
        _logger = logger;
        _courseCategoryRepository = courseCategoryRepository;
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CourseCategoryResponse>>> Handle(GetCourseCategoryByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<List<CourseCategoryResponse>>(
                    new Error("Course.NotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Get all course categories for the course
            var courseCategories = _courseCategoryRepository.FindAll(cc =>
                cc.CourseId == request.CourseId && !cc.IsDeleted)
                .OrderBy(cc => cc.CreatedAt)
                .ToList();

            // Create the response list
            var responseList = new List<CourseCategoryResponse>();

            foreach (var courseCategory in courseCategories)
            {
                // Get the category details
                var category = await _categoryRepository.FindByIdAsync(courseCategory.CategoryId, cancellationToken);
                if (category == null || category.IsDeleted) continue;

                // Add to response list
                responseList.Add(new CourseCategoryResponse(
                    courseCategory.Id,
                    course.Id,
                    course.Name,
                    category.Id,
                    category.Name,
                    courseCategory.CreatedAt,
                    courseCategory.UpdatedAt));
            }

            return Result.Success(responseList);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course categories by course ID: {ex.Message}");
            return Result.Failure<List<CourseCategoryResponse>>(
                new Error("CourseCategory.QueryFailed", $"Failed to query course categories: {ex.Message}"));
        }
    }
}