using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Lesson;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Lesson.Command;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Lesson;

public class CreateLessonCommandHandler : ICommandHandler<CreateLessonCommand, LessonResponse>
{
    private readonly ILogger<CreateLessonCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;
    private readonly IMapper _mapper;

    public CreateLessonCommandHandler(
        ILogger<CreateLessonCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonRepository = lessonRepository;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _sectionLessonRepository = sectionLessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<LessonResponse>> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the section exists
            var section = await _sectionRepository.FindByIdAsync(request.sectionId, cancellationToken);
            if (section == null)
            {
                return Result.Failure<LessonResponse>(
                    new Error("Lesson.SectionNotFound", $"Section with ID {request.sectionId} not found."));
            }

            // Verify that the course exists if courseId is provided
            if (request.courseId.HasValue)
            {
                var course = await _courseRepository.FindByIdAsync(request.courseId.Value, cancellationToken);
                if (course == null)
                {
                    return Result.Failure<LessonResponse>(
                        new Error("Lesson.CourseNotFound", $"Course with ID {request.courseId} not found."));
                }
            }

            // Create the lesson
            var lesson = Domain.Entities.Lesson.Create(
                request.title,
                request.description,
                request.videoUrl,
                request.pdfUrl,
                request.sectionId,
                request.courseId,
                request.duration,
                request.createdBy);

            // Add to repository
            _lessonRepository.Add(lesson);

            // Create the section-lesson relationship
            var sectionLesson = Domain.Entities.SectionLesson.Create(
                request.sectionId,
                lesson.Id,
                request.createdBy);

            _sectionLessonRepository.Add(sectionLesson);

            // Map to response and return
            var response = _mapper.Map<LessonResponse>(lesson);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating lesson: {ex.Message}");
            return Result.Failure<LessonResponse>(
                new Error("Lesson.CreateFailed", $"Failed to create lesson: {ex.Message}"));
        }
    }
}