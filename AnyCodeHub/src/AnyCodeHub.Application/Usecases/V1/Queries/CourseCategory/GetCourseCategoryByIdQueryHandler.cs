using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Query;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseCategory;

public class GetCourseCategoryByIdQueryHandler : IQueryHandler<GetCourseCategoryByIdQuery, CourseCategoryDetailResponse>
{
    private readonly ILogger<GetCourseCategoryByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseCategory, Guid> _courseCategoryRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;

    public GetCourseCategoryByIdQueryHandler(
        ILogger<GetCourseCategoryByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseCategory, Guid> courseCategoryRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    {
        _logger = logger;
        _courseCategoryRepository = courseCategoryRepository;
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CourseCategoryDetailResponse>> Handle(GetCourseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the course category by ID
            var courseCategory = await _courseCategoryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (courseCategory == null || courseCategory.IsDeleted)
            {
                return Result.Failure<CourseCategoryDetailResponse>(
                    new Error("CourseCategory.NotFound", $"Course category with ID {request.Id} not found."));
            }

            // Get course details
            var course = await _courseRepository.FindByIdAsync(courseCategory.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<CourseCategoryDetailResponse>(
                    new Error("Course.NotFound", $"Course with ID {courseCategory.CourseId} not found."));
            }

            // Get category details
            var category = await _categoryRepository.FindByIdAsync(courseCategory.CategoryId, cancellationToken);
            if (category == null || category.IsDeleted)
            {
                return Result.Failure<CourseCategoryDetailResponse>(
                    new Error("Category.NotFound", $"Category with ID {courseCategory.CategoryId} not found."));
            }

            // Create the response
            var response = new CourseCategoryDetailResponse(
                courseCategory.Id,
                course.Id,
                course.Name,
                course.Description,
                category.Id,
                category.Name,
                category.Description,
                courseCategory.CreatedAt,
                courseCategory.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course category by ID: {ex.Message}");
            return Result.Failure<CourseCategoryDetailResponse>(
                new Error("CourseCategory.QueryFailed", $"Failed to query course category: {ex.Message}"));
        }
    }
}