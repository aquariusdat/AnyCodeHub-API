using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Rating.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Rating;

public class DeleteRatingCommandHandler : ICommandHandler<DeleteRatingCommand, bool>
{
    private readonly ILogger<DeleteRatingCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Rating, Guid> _ratingRepository;

    public DeleteRatingCommandHandler(
        ILogger<DeleteRatingCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Rating, Guid> ratingRepository)
    {
        _logger = logger;
        _ratingRepository = ratingRepository;
    }

    public async Task<Result<bool>> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the rating exists
            var rating = await _ratingRepository.FindByIdAsync(request.id, cancellationToken);
            if (rating == null)
            {
                return Result.Failure<bool>(
                    new Error("Rating.NotFound", $"Rating with ID {request.id} not found."));
            }

            // Mark the rating as deleted
            rating.Delete(request.deletedBy);

            // Update the repository
            _ratingRepository.Update(rating);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting rating: {ex.Message}");
            return Result.Failure<bool>(
                new Error("Rating.DeleteFailed", $"Failed to delete rating: {ex.Message}"));
        }
    }
}