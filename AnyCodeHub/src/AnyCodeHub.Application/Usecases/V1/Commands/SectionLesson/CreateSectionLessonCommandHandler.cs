using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.SectionLesson;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.SectionLesson.Command;
using static AnyCodeHub.Contract.Services.V1.SectionLesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.SectionLesson;

public class CreateSectionLessonCommandHandler : ICommandHandler<CreateSectionLessonCommand, SectionLessonResponse>
{
    private readonly ILogger<CreateSectionLessonCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IMapper _mapper;

    public CreateSectionLessonCommandHandler(
        ILogger<CreateSectionLessonCommandHandler> logger,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _sectionLessonRepository = sectionLessonRepository;
        _sectionRepository = sectionRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<SectionLessonResponse>> Handle(CreateSectionLessonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify the section exists
            var section = await _sectionRepository.FindByIdAsync(request.sectionId, cancellationToken);
            if (section == null || section.IsDeleted)
            {
                return Result.Failure<SectionLessonResponse>(
                    new Error("Section.NotFound", $"Section with ID {request.sectionId} not found."));
            }

            // Verify the lesson exists
            var lesson = await _lessonRepository.FindByIdAsync(request.lessonId, cancellationToken);
            if (lesson == null || lesson.IsDeleted)
            {
                return Result.Failure<SectionLessonResponse>(
                    new Error("Lesson.NotFound", $"Lesson with ID {request.lessonId} not found."));
            }

            // Check if the section lesson already exists
            var existingSectionLesson = _sectionLessonRepository.FindAll(sl =>
                sl.SectionId == request.sectionId &&
                sl.LessonId == request.lessonId &&
                !sl.IsDeleted).FirstOrDefault();

            if (existingSectionLesson != null)
            {
                return Result.Failure<SectionLessonResponse>(
                    new Error("SectionLesson.AlreadyExists", "This lesson is already associated with this section."));
            }

            // Create new section lesson
            var sectionLesson = Domain.Entities.SectionLesson.Create(
                request.sectionId,
                request.lessonId,
                request.createdBy);

            // Add to repository
            _sectionLessonRepository.Add(sectionLesson);

            // Create response
            var response = new SectionLessonResponse(
                sectionLesson.Id,
                sectionLesson.SectionId,
                section.Name,
                sectionLesson.LessonId,
                lesson.Title,
                sectionLesson.CreatedAt,
                sectionLesson.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating section lesson: {ex.Message}");
            return Result.Failure<SectionLessonResponse>(
                new Error("SectionLesson.CreateFailed", $"Failed to create section lesson: {ex.Message}"));
        }
    }
}