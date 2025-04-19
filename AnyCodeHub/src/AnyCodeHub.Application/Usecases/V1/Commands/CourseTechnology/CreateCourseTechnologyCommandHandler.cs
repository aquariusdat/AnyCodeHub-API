using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.CourseTechnology;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseTechnology.Command;
using static AnyCodeHub.Contract.Services.V1.CourseTechnology.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseTechnology;

public class CreateCourseTechnologyCommandHandler : ICommandHandler<CreateCourseTechnologyCommand, CourseTechnologyResponse>
{
    private readonly ILogger<CreateCourseTechnologyCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseTechnology, Guid> _courseTechnologyRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Technology, Guid> _technologyRepository;
    private readonly IMapper _mapper;

    public CreateCourseTechnologyCommandHandler(
        ILogger<CreateCourseTechnologyCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseTechnology, Guid> courseTechnologyRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Technology, Guid> technologyRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseTechnologyRepository = courseTechnologyRepository;
        _courseRepository = courseRepository;
        _technologyRepository = technologyRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseTechnologyResponse>> Handle(CreateCourseTechnologyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<CourseTechnologyResponse>(
                    new Error("CourseTechnology.CourseNotFound", $"Course with ID {request.courseId} not found."));
            }

            // Verify that the technology exists
            var technology = await _technologyRepository.FindByIdAsync(request.technologyId, cancellationToken);
            if (technology == null)
            {
                return Result.Failure<CourseTechnologyResponse>(
                    new Error("CourseTechnology.TechnologyNotFound", $"Technology with ID {request.technologyId} not found."));
            }

            // Check if the association already exists
            var existingAssociation = _courseTechnologyRepository.FindAll(ct =>
                ct.CourseId == request.courseId &&
                ct.TechnologyId == request.technologyId &&
                !ct.IsDeleted).FirstOrDefault();

            if (existingAssociation != null)
            {
                return Result.Failure<CourseTechnologyResponse>(
                    new Error("CourseTechnology.AlreadyExists", "This technology is already associated with the course."));
            }

            // Create the association
            var courseTechnology = Domain.Entities.CourseTechnology.Create(
                request.courseId,
                request.technologyId);

            // Set audit fields
            courseTechnology.CreatedBy = request.createdBy;
            courseTechnology.CreatedAt = DateTime.UtcNow;

            // Add to repository
            _courseTechnologyRepository.Add(courseTechnology);

            // Create response
            var response = new CourseTechnologyResponse(
                courseTechnology.Id,
                courseTechnology.CourseId,
                course.Name,
                courseTechnology.TechnologyId,
                technology.Name,
                courseTechnology.CreatedAt,
                courseTechnology.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating course technology association: {ex.Message}");
            return Result.Failure<CourseTechnologyResponse>(
                new Error("CourseTechnology.CreateFailed", $"Failed to create course technology association: {ex.Message}"));
        }
    }
}