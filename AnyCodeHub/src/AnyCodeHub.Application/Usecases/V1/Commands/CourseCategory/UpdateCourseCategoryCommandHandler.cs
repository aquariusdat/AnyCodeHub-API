using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseCategory;

public class UpdateCourseCategoryCommandHandler : ICommandHandler<UpdateCourseCategoryCommand, bool>
{
    private readonly ILogger<UpdateCourseCategoryCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseCategory, Guid> _courseCategoryRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;

    public UpdateCourseCategoryCommandHandler(
        ILogger<UpdateCourseCategoryCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseCategory, Guid> courseCategoryRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    {
        _logger = logger;
        _courseCategoryRepository = courseCategoryRepository;
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<bool>> Handle(UpdateCourseCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify the course category exists
            var courseCategory = await _courseCategoryRepository.FindByIdAsync(request.id, cancellationToken);
            if (courseCategory == null || courseCategory.IsDeleted)
            {
                return Result.Failure<bool>(
                    new Error("CourseCategory.NotFound", $"Course category with ID {request.id} not found."));
            }

            // Check if CourseId or CategoryId has changed
            if (courseCategory.CourseId != request.courseId || courseCategory.CategoryId != request.categoryId)
            {
                // Since we can't modify CourseId and CategoryId directly using property setters, 
                // we'll use an alternative approach

                // Verify that the course exists
                var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
                if (course == null || course.IsDeleted)
                {
                    return Result.Failure<bool>(
                        new Error("Course.NotFound", $"Course with ID {request.courseId} not found."));
                }

                // Verify that the category exists
                var category = await _categoryRepository.FindByIdAsync(request.categoryId, cancellationToken);
                if (category == null || category.IsDeleted)
                {
                    return Result.Failure<bool>(
                        new Error("Category.NotFound", $"Category with ID {request.categoryId} not found."));
                }

                // Delete the old course category
                courseCategory.Delete(request.updatedBy);
                _courseCategoryRepository.Update(courseCategory);

                // Create a new course category using the factory method
                var newCourseCategory = Domain.Entities.CourseCategory.Create(
                    request.courseId,
                    request.categoryId,
                    request.updatedBy);

                // Add the new course category
                _courseCategoryRepository.Add(newCourseCategory);
            }
            else
            {
                // Use the entity's Update method to update properties
                courseCategory.Update(
                    courseCategory.Id,
                    courseCategory.CourseId,
                    courseCategory.CategoryId,
                    request.updatedBy);

                // Update in the repository
                _courseCategoryRepository.Update(courseCategory);
            }

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating course category: {ex.Message}");
            return Result.Failure<bool>(
                new Error("CourseCategory.UpdateFailed", $"Failed to update course category: {ex.Message}"));
        }
    }
}