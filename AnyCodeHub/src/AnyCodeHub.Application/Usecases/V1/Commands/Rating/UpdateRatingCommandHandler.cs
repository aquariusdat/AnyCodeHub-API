using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Rating;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AnyCodeHub.Domain.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Rating.Command;
using static AnyCodeHub.Contract.Services.V1.Rating.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Rating;

public class UpdateRatingCommandHandler : ICommandHandler<UpdateRatingCommand, RatingResponse>
{
    private readonly ILogger<UpdateRatingCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Rating, Guid> _ratingRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateRatingCommandHandler(
        ILogger<UpdateRatingCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Rating, Guid> ratingRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        UserManager<AppUser> userManager,
        IMapper mapper)
    {
        _logger = logger;
        _ratingRepository = ratingRepository;
        _courseRepository = courseRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result<RatingResponse>> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the rating exists
            var rating = await _ratingRepository.FindByIdAsync(request.id, cancellationToken);
            if (rating == null || rating.IsDeleted)
            {
                return Result.Failure<RatingResponse>(
                    new Error("Rating.NotFound", $"Rating with ID {request.id} not found."));
            }

            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<RatingResponse>(
                    new Error("Rating.CourseNotFound", $"Course with ID {request.courseId} not found."));
            }

            // Verify that the user exists
            var user = await _userManager.FindByIdAsync(request.userId.ToString());
            if (user == null)
            {
                return Result.Failure<RatingResponse>(
                    new Error("Rating.UserNotFound", $"User with ID {request.userId} not found."));
            }

            // Validate rating value
            if (request.rate < 1 || request.rate > 5)
            {
                return Result.Failure<RatingResponse>(
                    new Error("Rating.InvalidValue", "Rating must be between 1 and 5."));
            }

            // Update the rating
            rating.Update(
                request.id,
                request.courseId,
                request.userId,
                request.rate,
                request.updatedBy);

            // Update in repository
            _ratingRepository.Update(rating);

            // Create response
            var response = new RatingResponse(
                rating.Id,
                rating.CourseId,
                course.Name,
                rating.UserId,
                user.UserName,
                rating.Rate,
                rating.CreatedAt,
                rating.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating rating: {ex.Message}");
            return Result.Failure<RatingResponse>(
                new Error("Rating.UpdateFailed", $"Failed to update rating: {ex.Message}"));
        }
    }
}