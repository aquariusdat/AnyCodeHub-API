using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseBenefit;

public class DeleteCourseBenefitCommandHandler : ICommandHandler<DeleteCourseBenefitCommand, bool>
{
    private readonly ILogger<DeleteCourseBenefitCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseBenefit, Guid> _courseBenefitRepository;

    public DeleteCourseBenefitCommandHandler(
        ILogger<DeleteCourseBenefitCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseBenefit, Guid> courseBenefitRepository)
    {
        _logger = logger;
        _courseBenefitRepository = courseBenefitRepository;
    }

    public async Task<Result<bool>> Handle(DeleteCourseBenefitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course benefit exists
            var courseBenefit = await _courseBenefitRepository.FindByIdAsync(request.id, cancellationToken);
            if (courseBenefit == null)
            {
                return Result.Failure<bool>(
                    new Error("CourseBenefit.NotFound", $"Course benefit with ID {request.id} not found."));
            }

            // Mark the course benefit as deleted
            courseBenefit.IsDeleted = true;
            courseBenefit.DeletedAt = DateTime.UtcNow;
            courseBenefit.DeletedBy = request.deletedBy;

            // Update the repository
            _courseBenefitRepository.Update(courseBenefit);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting course benefit: {ex.Message}");
            return Result.Failure<bool>(
                new Error("CourseBenefit.DeleteFailed", $"Failed to delete course benefit: {ex.Message}"));
        }
    }
}