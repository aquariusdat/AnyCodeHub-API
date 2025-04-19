using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Section.Query;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Section;

public class GetSectionByIdQueryHandler : IQueryHandler<GetSectionByIdQuery, SectionDetailResponse>
{
    private readonly ILogger<GetSectionByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;

    public GetSectionByIdQueryHandler(
        ILogger<GetSectionByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _sectionLessonRepository = sectionLessonRepository;
    }

    public async Task<Result<SectionDetailResponse>> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the section by ID
            var section = await _sectionRepository.FindByIdAsync(request.Id, cancellationToken);
            if (section == null || section.IsDeleted)
            {
                return Result.Failure<SectionDetailResponse>(
                    new Error("Section.NotFound", $"Section with ID {request.Id} not found."));
            }

            // Get course details
            var course = await _courseRepository.FindByIdAsync(section.CourseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<SectionDetailResponse>(
                    new Error("Course.NotFound", $"Course with ID {section.CourseId} not found."));
            }

            // Get lessons for this section
            var sectionLessons = _sectionLessonRepository.FindAll(sl =>
                sl.SectionId == section.Id && !sl.IsDeleted)
                .OrderBy(sl => sl.CreatedAt)
                .ToList();

            var lessonInfos = new List<LessonInfo>();

            foreach (var sectionLesson in sectionLessons)
            {
                var lesson = await _lessonRepository.FindByIdAsync(sectionLesson.LessonId, cancellationToken);
                if (lesson == null || lesson.IsDeleted) continue;

                lessonInfos.Add(new LessonInfo(
                    lesson.Id,
                    lesson.Title,
                    lesson.Description,
                    lesson.VideoUrl,
                    lesson.PdfUrl,
                    lesson.Duration
                ));
            }

            // Create the response
            var response = new SectionDetailResponse(
                section.Id,
                section.Name,
                course.Id,
                course.Name,
                lessonInfos,
                section.CreatedAt,
                section.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting section by ID: {ex.Message}");
            return Result.Failure<SectionDetailResponse>(
                new Error("Section.QueryFailed", $"Failed to query section: {ex.Message}"));
        }
    }
}