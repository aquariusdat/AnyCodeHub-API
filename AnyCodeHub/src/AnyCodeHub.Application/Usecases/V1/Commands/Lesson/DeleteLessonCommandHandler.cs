using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Lesson.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Lesson;

public class DeleteLessonCommandHandler : ICommandHandler<DeleteLessonCommand, bool>
{
    private readonly ILogger<DeleteLessonCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;
    private readonly IRepositoryBase<Domain.Entities.LessonComment, Guid> _lessonCommentRepository;

    public DeleteLessonCommandHandler(
        ILogger<DeleteLessonCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository,
        IRepositoryBase<Domain.Entities.LessonComment, Guid> lessonCommentRepository)
    {
        _logger = logger;
        _lessonRepository = lessonRepository;
        _sectionLessonRepository = sectionLessonRepository;
        _lessonCommentRepository = lessonCommentRepository;
    }

    public async Task<Result<bool>> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the lesson exists
            var lesson = await _lessonRepository.FindByIdAsync(request.id, cancellationToken);
            if (lesson == null)
            {
                return Result.Failure<bool>(
                    new Error("Lesson.NotFound", $"Lesson with ID {request.id} not found."));
            }

            // Check for section-lesson relations
            var sectionLessons = _sectionLessonRepository.FindAll(sl => sl.LessonId == request.id).ToList();

            // Check for lesson comments
            var comments = _lessonCommentRepository.FindAll(lc => lc.LessonId == request.id).ToList();

            // Mark the lesson as deleted
            lesson.Delete(request.deletedBy);

            // Update the lesson in the repository
            _lessonRepository.Update(lesson);

            // We don't delete the relationships or comments, just mark the lesson as deleted
            // This ensures data integrity and allows for potential undeletion

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting lesson: {ex.Message}");
            return Result.Failure<bool>(
                new Error("Lesson.DeleteFailed", $"Failed to delete lesson: {ex.Message}"));
        }
    }
}