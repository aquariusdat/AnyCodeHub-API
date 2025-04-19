using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Lesson.Query;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Lesson;

public class GetLessonsByCourseIdQueryHandler : IQueryHandler<GetLessonsByCourseIdQuery, IEnumerable<LessonResponse>>
{
    private readonly ILogger<GetLessonsByCourseIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public GetLessonsByCourseIdQueryHandler(
        ILogger<GetLessonsByCourseIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<LessonResponse>>> Handle(GetLessonsByCourseIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<IEnumerable<LessonResponse>>(
                    new Error("Lesson.CourseNotFound", $"Course with ID {request.CourseId} not found."));
            }

            // Get all lessons for this course
            var lessons = _lessonRepository.FindAll(l => l.CourseId == request.CourseId && !l.IsDeleted)
                .OrderBy(l => l.Title)
                .ToList();

            // Map to response DTOs
            var lessonResponses = _mapper.Map<List<LessonResponse>>(lessons);

            return Result.Success<IEnumerable<LessonResponse>>(lessonResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting lessons by course ID: {ex.Message}");
            return Result.Failure<IEnumerable<LessonResponse>>(
                new Error("Lesson.QueryFailed", $"Failed to query lessons: {ex.Message}"));
        }
    }
}