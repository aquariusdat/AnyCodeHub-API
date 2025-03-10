using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Contract.Services.V1.Course;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Course;

public class CreateCourseCommandHandler : ICommandHandler<Command.CreateCourseCommand, Response.CourseResponse>
{
    private readonly ILogger<CreateCourseCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _repository;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(ILogger<CreateCourseCommandHandler> logger, IRepositoryBase<Domain.Entities.Course, Guid> repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Response.CourseResponse>> Handle(Command.CreateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if a course with the same name or slug already exists
            var existingCourse = _repository.FindAll(c =>
                c.Name.ToLower() == request.name.ToLower() ||
                (!string.IsNullOrEmpty(request.slug) && !string.IsNullOrEmpty(c.Slug) && c.Slug.ToLower() == request.slug.ToLower())
            ).FirstOrDefault();

            if (existingCourse != null)
            {
                // A course with the same name or slug already exists
                string duplicateField = existingCourse.Name.ToLower() == request.name.ToLower() ? "name" : "slug";
                return Result.Failure<Response.CourseResponse>(
                    new Error("Course.Duplicate", $"A course with this {duplicateField} already exists."));
            }

            // Create a new course entity
            var course = Domain.Entities.Course.Create(
                request.name,
                request.description,
                request.price,
                request.salePrice,
                request.imageUrl,
                request.videoUrl,
                request.slug,
                request.status,
                request.authorId,
                request.level,
                request.totalViews,
                request.totalDuration,
                request.rating,
                request.createdBy);

            // Add to repository
            _repository.Add(course);

            // Map to response and return
            var response = _mapper.Map<Response.CourseResponse>(course);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating course: {ex.Message}");
            return Result.Failure<Response.CourseResponse>(
                new Error("Course.CreateFailed", $"Failed to create course: {ex.Message}"));
        }
    }
}
