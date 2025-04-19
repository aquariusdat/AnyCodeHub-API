using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.CourseQA;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Command;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseQA;

public class UpdateCourseQACommandHandler : ICommandHandler<UpdateCourseQACommand, CourseQAResponse>
{
    private readonly ILogger<UpdateCourseQACommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseQA, Guid> _courseQARepository;
    private readonly IMapper _mapper;

    public UpdateCourseQACommandHandler(
        ILogger<UpdateCourseQACommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseQA, Guid> courseQARepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseQARepository = courseQARepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseQAResponse>> Handle(UpdateCourseQACommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course QA exists
            var courseQA = await _courseQARepository.FindByIdAsync(request.id, cancellationToken);
            if (courseQA == null || courseQA.IsDeleted)
            {
                return Result.Failure<CourseQAResponse>(
                    new Error("CourseQA.NotFound", $"Course QA with ID {request.id} not found."));
            }

            // Update the course QA
            courseQA.Update(
                request.id,
                request.question,
                request.answer,
                courseQA.CourseId);

            // Set audit fields
            courseQA.UpdatedBy = request.updatedBy;
            courseQA.UpdatedAt = DateTime.UtcNow;

            // Update in repository
            _courseQARepository.Update(courseQA);

            // Create response
            var response = new CourseQAResponse(
                courseQA.Id,
                courseQA.Question,
                courseQA.Answer,
                courseQA.CourseId,
                courseQA.CreatedAt,
                courseQA.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating course QA: {ex.Message}");
            return Result.Failure<CourseQAResponse>(
                new Error("CourseQA.UpdateFailed", $"Failed to update course QA: {ex.Message}"));
        }
    }
}