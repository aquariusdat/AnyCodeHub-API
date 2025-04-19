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

public class UpdateLessonCommandHandler : ICommandHandler<UpdateLessonCommand, LessonResponse>
{
    private readonly ILogger<UpdateLessonCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;
    private readonly IMapper _mapper;

    public UpdateLessonCommandHandler(
        ILogger<UpdateLessonCommandHandler> logger,
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

    public async Task<Result<LessonResponse>> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the lesson exists
            var lesson = await _lessonRepository.FindByIdAsync(request.id, cancellationToken);
            if (lesson == null)
            {
                return Result.Failure<LessonResponse>(
                    new Error("Lesson.NotFound", $"Lesson with ID {request.id} not found."));
            }

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

            // Check if section has changed
            bool sectionChanged = lesson.SectionId != request.sectionId;

            // Update the lesson
            lesson.Update(
                request.id,
                request.title,
                request.description,
                request.videoUrl,
                request.pdfUrl,
                request.sectionId,
                request.courseId,
                request.duration,
                request.updatedBy);

            // Update the lesson in the repository
            _lessonRepository.Update(lesson);

            // If section has changed, update the section-lesson relationship
            if (sectionChanged)
            {
                // Remove old relationship
                var oldSectionLesson = _sectionLessonRepository.FindAll(sl => sl.LessonId == lesson.Id).FirstOrDefault();
                if (oldSectionLesson != null)
                {
                    _sectionLessonRepository.Remove(oldSectionLesson);
                }

                // Create new relationship
                var newSectionLesson = Domain.Entities.SectionLesson.Create(
                    request.sectionId,
                    lesson.Id,
                    request.updatedBy);

                _sectionLessonRepository.Add(newSectionLesson);
            }

            // Map to response and return
            var response = _mapper.Map<LessonResponse>(lesson);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating lesson: {ex.Message}");
            return Result.Failure<LessonResponse>(
                new Error("Lesson.UpdateFailed", $"Failed to update lesson: {ex.Message}"));
        }
    }
}