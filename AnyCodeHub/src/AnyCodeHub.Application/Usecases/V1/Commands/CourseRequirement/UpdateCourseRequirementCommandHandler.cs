using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.CourseRequirement;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Command;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseRequirement;

public class UpdateCourseRequirementCommandHandler : ICommandHandler<UpdateCourseRequirementCommand, CourseRequirementResponse>
{
    private readonly ILogger<UpdateCourseRequirementCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseRequirement, Guid> _courseRequirementRepository;
    private readonly IMapper _mapper;

    public UpdateCourseRequirementCommandHandler(
        ILogger<UpdateCourseRequirementCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseRequirement, Guid> courseRequirementRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseRequirementRepository = courseRequirementRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseRequirementResponse>> Handle(UpdateCourseRequirementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course requirement exists
            var courseRequirement = await _courseRequirementRepository.FindByIdAsync(request.id, cancellationToken);
            if (courseRequirement == null || courseRequirement.IsDeleted)
            {
                return Result.Failure<CourseRequirementResponse>(
                    new Error("CourseRequirement.NotFound", $"Course requirement with ID {request.id} not found."));
            }

            // Update the course requirement
            courseRequirement.Update(request.id, request.requirementContent);

            // Update in repository
            _courseRequirementRepository.Update(courseRequirement);

            // Create response
            var response = new CourseRequirementResponse(
                courseRequirement.Id,
                courseRequirement.RequirementContent,
                courseRequirement.CourseId,
                courseRequirement.CreatedAt,
                courseRequirement.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating course requirement: {ex.Message}");
            return Result.Failure<CourseRequirementResponse>(
                new Error("CourseRequirement.UpdateFailed", $"Failed to update course requirement: {ex.Message}"));
        }
    }
}