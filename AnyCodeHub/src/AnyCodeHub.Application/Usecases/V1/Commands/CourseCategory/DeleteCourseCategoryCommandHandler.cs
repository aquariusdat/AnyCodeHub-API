using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseCategory;

public class DeleteCourseCategoryCommandHandler : ICommandHandler<DeleteCourseCategoryCommand, bool>
{
    private readonly ILogger<DeleteCourseCategoryCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseCategory, Guid> _courseCategoryRepository;

    public DeleteCourseCategoryCommandHandler(
        ILogger<DeleteCourseCategoryCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseCategory, Guid> courseCategoryRepository)
    {
        _logger = logger;
        _courseCategoryRepository = courseCategoryRepository;
    }

    public async Task<Result<bool>> Handle(DeleteCourseCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course category association exists
            var courseCategory = await _courseCategoryRepository.FindByIdAsync(request.id, cancellationToken);
            if (courseCategory == null || courseCategory.IsDeleted)
            {
                return Result.Failure<bool>(
                    new Error("CourseCategory.NotFound", $"Course category association with ID {request.id} not found."));
            }

            // Mark the course category as deleted
            courseCategory.Delete(request.deletedBy);

            // Update in repository
            _courseCategoryRepository.Update(courseCategory);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting course category association: {ex.Message}");
            return Result.Failure<bool>(
                new Error("CourseCategory.DeleteFailed", $"Failed to delete course category association: {ex.Message}"));
        }
    }
}