using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.SectionLesson.Query;
using static AnyCodeHub.Contract.Services.V1.SectionLesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.SectionLesson;

public class GetSectionLessonByIdQueryHandler : IQueryHandler<GetSectionLessonByIdQuery, SectionLessonDetailResponse>
{
    private readonly ILogger<GetSectionLessonByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;

    public GetSectionLessonByIdQueryHandler(
        ILogger<GetSectionLessonByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository)
    {
        _logger = logger;
        _sectionLessonRepository = sectionLessonRepository;
        _sectionRepository = sectionRepository;
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Result<SectionLessonDetailResponse>> Handle(GetSectionLessonByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the section lesson by ID
            var sectionLesson = await _sectionLessonRepository.FindByIdAsync(request.Id, cancellationToken);
            if (sectionLesson == null || sectionLesson.IsDeleted)
            {
                return Result.Failure<SectionLessonDetailResponse>(
                    new Error("SectionLesson.NotFound", $"Section lesson with ID {request.Id} not found."));
            }

            // Get section details
            var section = await _sectionRepository.FindByIdAsync(sectionLesson.SectionId, cancellationToken);
            if (section == null || section.IsDeleted)
            {
                return Result.Failure<SectionLessonDetailResponse>(
                    new Error("Section.NotFound", $"Section with ID {sectionLesson.SectionId} not found."));
            }

            // Get course details
            var course = await _courseRepository.FindByIdAsync(section.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<SectionLessonDetailResponse>(
                    new Error("Course.NotFound", $"Course with ID {section.CourseId} not found."));
            }

            // Get lesson details
            var lesson = await _lessonRepository.FindByIdAsync(sectionLesson.LessonId, cancellationToken);
            if (lesson == null || lesson.IsDeleted)
            {
                return Result.Failure<SectionLessonDetailResponse>(
                    new Error("Lesson.NotFound", $"Lesson with ID {sectionLesson.LessonId} not found."));
            }

            // Create response
            var response = new SectionLessonDetailResponse(
                sectionLesson.Id,
                section.Id,
                section.Name,
                course.Id,
                course.Name,
                lesson.Id,
                lesson.Title,
                lesson.Description,
                lesson.VideoUrl,
                lesson.PdfUrl,
                lesson.Duration,
                sectionLesson.CreatedAt,
                sectionLesson.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting section lesson by ID: {ex.Message}");
            return Result.Failure<SectionLessonDetailResponse>(
                new Error("SectionLesson.QueryFailed", $"Failed to query section lesson: {ex.Message}"));
        }
    }
}