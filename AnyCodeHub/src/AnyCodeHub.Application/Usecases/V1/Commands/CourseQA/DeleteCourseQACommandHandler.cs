using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseQA;

public class DeleteCourseQACommandHandler : ICommandHandler<DeleteCourseQACommand, bool>
{
    private readonly ILogger<DeleteCourseQACommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseQA, Guid> _courseQARepository;

    public DeleteCourseQACommandHandler(
        ILogger<DeleteCourseQACommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseQA, Guid> courseQARepository)
    {
        _logger = logger;
        _courseQARepository = courseQARepository;
    }

    public async Task<Result<bool>> Handle(DeleteCourseQACommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify the course QA exists
            var courseQA = await _courseQARepository.FindByIdAsync(request.id, cancellationToken);
            if (courseQA == null || courseQA.IsDeleted)
            {
                return Result.Failure<bool>(
                    new Error("CourseQA.NotFound", $"Course QA with ID {request.id} not found."));
            }

            // Soft delete the course QA
            courseQA.IsDeleted = true;
            courseQA.DeletedAt = DateTime.UtcNow;
            courseQA.DeletedBy = request.deletedBy;

            // Update the repository
            _courseQARepository.Update(courseQA);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting course QA: {ex.Message}");
            return Result.Failure<bool>(
                new Error("CourseQA.DeleteFailed", $"Failed to delete course QA: {ex.Message}"));
        }
    }
}