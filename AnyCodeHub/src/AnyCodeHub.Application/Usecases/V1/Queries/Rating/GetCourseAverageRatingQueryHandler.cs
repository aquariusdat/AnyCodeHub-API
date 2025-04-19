using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Rating.Query;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Rating;

public class GetCourseAverageRatingQueryHandler : IQueryHandler<GetCourseAverageRatingQuery, double>
{
    private readonly ILogger<GetCourseAverageRatingQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Rating, Guid> _ratingRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;

    public GetCourseAverageRatingQueryHandler(
        ILogger<GetCourseAverageRatingQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Rating, Guid> ratingRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository)
    {
        _logger = logger;
        _ratingRepository = ratingRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Result<double>> Handle(GetCourseAverageRatingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<double>(
                    new Error("Rating.CourseNotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Get all ratings for this course
            var ratings = _ratingRepository.FindAll(r => r.CourseId == request.CourseId && !r.IsDeleted).ToList();

            // Calculate average
            if (ratings.Count == 0)
            {
                return Result.Success(0.0); // No ratings yet
            }

            var average = ratings.Average(r => r.Rate);
            return Result.Success(average);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course average rating: {ex.Message}");
            return Result.Failure<double>(
                new Error("Rating.QueryFailed", $"Failed to query average rating: {ex.Message}"));
        }
    }
}