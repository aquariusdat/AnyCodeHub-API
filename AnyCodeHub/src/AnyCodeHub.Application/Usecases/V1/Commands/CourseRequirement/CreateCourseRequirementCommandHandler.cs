using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.CourseRequirement;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Command;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseRequirement;

public class CreateCourseRequirementCommandHandler : ICommandHandler<CreateCourseRequirementCommand, CourseRequirementResponse>
{
    private readonly ILogger<CreateCourseRequirementCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseRequirement, Guid> _courseRequirementRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public CreateCourseRequirementCommandHandler(
        ILogger<CreateCourseRequirementCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseRequirement, Guid> courseRequirementRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseRequirementRepository = courseRequirementRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseRequirementResponse>> Handle(CreateCourseRequirementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<CourseRequirementResponse>(
                    new Error("CourseRequirement.CourseNotFound", $"Course with ID {request.courseId} not found."));
            }

            // Create the course requirement
            var courseRequirement = Domain.Entities.CourseRequirement.Create(
                request.requirementContent,
                request.courseId,
                request.createdBy);

            // Add to repository
            _courseRequirementRepository.Add(courseRequirement);

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
            _logger.LogError($"Error while creating course requirement: {ex.Message}");
            return Result.Failure<CourseRequirementResponse>(
                new Error("CourseRequirement.CreateFailed", $"Failed to create course requirement: {ex.Message}"));
        }
    }
}