using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseRequirement;

public class DeleteCourseRequirementCommandHandler : ICommandHandler<DeleteCourseRequirementCommand, bool>
{
    private readonly ILogger<DeleteCourseRequirementCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseRequirement, Guid> _courseRequirementRepository;

    public DeleteCourseRequirementCommandHandler(
        ILogger<DeleteCourseRequirementCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseRequirement, Guid> courseRequirementRepository)
    {
        _logger = logger;
        _courseRequirementRepository = courseRequirementRepository;
    }

    public async Task<Result<bool>> Handle(DeleteCourseRequirementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course requirement exists
            var courseRequirement = await _courseRequirementRepository.FindByIdAsync(request.id, cancellationToken);
            if (courseRequirement == null || courseRequirement.IsDeleted)
            {
                return Result.Failure<bool>(
                    new Error("CourseRequirement.NotFound", $"Course requirement with ID {request.id} not found."));
            }

            // Soft delete the course requirement
            courseRequirement.IsDeleted = true;
            courseRequirement.DeletedAt = DateTime.UtcNow;
            courseRequirement.DeletedBy = request.deletedBy;

            // Update the repository
            _courseRequirementRepository.Update(courseRequirement);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting course requirement: {ex.Message}");
            return Result.Failure<bool>(
                new Error("CourseRequirement.DeleteFailed", $"Failed to delete course requirement: {ex.Message}"));
        }
    }
}