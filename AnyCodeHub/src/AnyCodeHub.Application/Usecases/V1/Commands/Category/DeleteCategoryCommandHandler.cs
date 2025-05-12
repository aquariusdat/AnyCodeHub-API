using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Category.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Category;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
{
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IRepositoryBase<Domain.Entities.CourseCategory, Guid> _courseCategoryRepository;

    public DeleteCategoryCommandHandler(
        ILogger<DeleteCategoryCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
        IRepositoryBase<Domain.Entities.CourseCategory, Guid> courseCategoryRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _courseCategoryRepository = courseCategoryRepository;
    }
    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the category exists
            var category = await _categoryRepository.FindByIdAsync(request.id, cancellationToken);
            if (category == null || category.IsDeleted)
            {
                return Result.Failure<bool>(
                    new Error("Category.NotFound", $"Category with ID {request.id} not found."));
            }

            // Check if there are any courses using this category
            var courseCategories = _courseCategoryRepository.FindAll(cc => cc.CategoryId == request.id && !cc.IsDeleted).ToList();
            if (courseCategories.Any())
            {
                return Result.Failure<bool>(
                    new Error("Category.InUse", $"Cannot delete category with ID {request.id} because it is in use by {courseCategories.Count} courses."));
            }

            // Mark the category as deleted
            category.Delete(request.deletedBy);

            // Update in repository
            _categoryRepository.Update(category);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting category: {ex.Message}");
            return Result.Failure<bool>(
                new Error("Category.DeleteFailed", $"Failed to delete category: {ex.Message}"));
        }
    }
}