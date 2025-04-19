using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.CourseQA;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Command;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseQA;

public class CreateCourseQACommandHandler : ICommandHandler<CreateCourseQACommand, CourseQAResponse>
{
    private readonly ILogger<CreateCourseQACommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseQA, Guid> _courseQARepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public CreateCourseQACommandHandler(
        ILogger<CreateCourseQACommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseQA, Guid> courseQARepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseQARepository = courseQARepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseQAResponse>> Handle(CreateCourseQACommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<CourseQAResponse>(
                    new Error("CourseQA.CourseNotFound", $"Course with ID {request.courseId} not found."));
            }

            // Create the course QA
            var courseQA = Domain.Entities.CourseQA.Create(
                request.question,
                request.answer,
                request.courseId);

            // Set audit fields
            courseQA.CreatedBy = request.createdBy;
            courseQA.CreatedAt = DateTime.UtcNow;

            // Add to repository
            _courseQARepository.Add(courseQA);

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
            _logger.LogError($"Error while creating course QA: {ex.Message}");
            return Result.Failure<CourseQAResponse>(
                new Error("CourseQA.CreateFailed", $"Failed to create course QA: {ex.Message}"));
        }
    }
}