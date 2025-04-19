using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Lesson.Query;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Lesson;

public class GetLessonsBySectionIdQueryHandler : IQueryHandler<GetLessonsBySectionIdQuery, IEnumerable<LessonResponse>>
{
    private readonly ILogger<GetLessonsBySectionIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;
    private readonly IMapper _mapper;

    public GetLessonsBySectionIdQueryHandler(
        ILogger<GetLessonsBySectionIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonRepository = lessonRepository;
        _sectionRepository = sectionRepository;
        _sectionLessonRepository = sectionLessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<LessonResponse>>> Handle(GetLessonsBySectionIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the section exists
            var section = await _sectionRepository.FindByIdAsync(request.SectionId, cancellationToken);
            if (section == null || section.IsDeleted)
            {
                return Result.Failure<IEnumerable<LessonResponse>>(
                    new Error("Lesson.SectionNotFound", $"Section with ID {request.SectionId} not found."));
            }

            // Get all lessons for this section using SectionLesson mapping
            var sectionLessons = _sectionLessonRepository.FindAll(sl => sl.SectionId == request.SectionId).ToList();

            if (!sectionLessons.Any())
            {
                // Return empty list if no lessons are found
                return Result.Success<IEnumerable<LessonResponse>>(new List<LessonResponse>());
            }

            // Get the lesson IDs from the section-lesson mappings
            var lessonIds = sectionLessons.Select(sl => sl.LessonId).ToList();

            // Retrieve all lessons
            var lessons = _lessonRepository.FindAll(l => lessonIds.Contains(l.Id) && !l.IsDeleted).ToList();

            // Map to response DTOs
            var lessonResponses = _mapper.Map<List<LessonResponse>>(lessons);

            return Result.Success<IEnumerable<LessonResponse>>(lessonResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting lessons by section ID: {ex.Message}");
            return Result.Failure<IEnumerable<LessonResponse>>(
                new Error("Lesson.QueryFailed", $"Failed to query lessons: {ex.Message}"));
        }
    }
}