using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.LessonComment;

public class DeleteLessonCommentCommandHandler : ICommandHandler<DeleteLessonCommentCommand, bool>
{
    private readonly ILogger<DeleteLessonCommentCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.LessonComment, Guid> _lessonCommentRepository;

    public DeleteLessonCommentCommandHandler(
        ILogger<DeleteLessonCommentCommandHandler> logger,
        IRepositoryBase<Domain.Entities.LessonComment, Guid> lessonCommentRepository)
    {
        _logger = logger;
        _lessonCommentRepository = lessonCommentRepository;
    }

    public async Task<Result<bool>> Handle(DeleteLessonCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the lesson comment exists
            var lessonComment = await _lessonCommentRepository.FindByIdAsync(request.id, cancellationToken);
            if (lessonComment == null)
            {
                return Result.Failure<bool>(
                    new Error("LessonComment.NotFound", $"Lesson comment with ID {request.id} not found."));
            }

            // Mark the lesson comment as deleted
            lessonComment.Delete(request.deletedBy);

            // Update the repository
            _lessonCommentRepository.Update(lessonComment);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting lesson comment: {ex.Message}");
            return Result.Failure<bool>(
                new Error("LessonComment.DeleteFailed", $"Failed to delete lesson comment: {ex.Message}"));
        }
    }
}