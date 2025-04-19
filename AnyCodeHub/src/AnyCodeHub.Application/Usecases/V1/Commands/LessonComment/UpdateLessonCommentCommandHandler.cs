using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.LessonComment;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Command;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.LessonComment;

public class UpdateLessonCommentCommandHandler : ICommandHandler<UpdateLessonCommentCommand, LessonCommentResponse>
{
    private readonly ILogger<UpdateLessonCommentCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.LessonComment, Guid> _lessonCommentRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IMapper _mapper;

    public UpdateLessonCommentCommandHandler(
        ILogger<UpdateLessonCommentCommandHandler> logger,
        IRepositoryBase<Domain.Entities.LessonComment, Guid> lessonCommentRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonCommentRepository = lessonCommentRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<LessonCommentResponse>> Handle(UpdateLessonCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the lesson comment exists
            var lessonComment = await _lessonCommentRepository.FindByIdAsync(request.id, cancellationToken);
            if (lessonComment == null || lessonComment.IsDeleted)
            {
                return Result.Failure<LessonCommentResponse>(
                    new Error("LessonComment.NotFound", $"Lesson comment with ID {request.id} not found."));
            }

            // Verify that the lesson exists
            var lesson = await _lessonRepository.FindByIdAsync(request.lessonId, cancellationToken);
            if (lesson == null || lesson.IsDeleted)
            {
                return Result.Failure<LessonCommentResponse>(
                    new Error("LessonComment.LessonNotFound", $"Lesson with ID {request.lessonId} not found."));
            }

            // Update the lesson comment
            lessonComment.Update(
                request.id,
                request.lessonId,
                request.commentId,
                request.content,
                request.updatedBy);

            // Update the repository
            _lessonCommentRepository.Update(lessonComment);

            // Map to response and return
            var response = _mapper.Map<LessonCommentResponse>(lessonComment);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating lesson comment: {ex.Message}");
            return Result.Failure<LessonCommentResponse>(
                new Error("LessonComment.UpdateFailed", $"Failed to update lesson comment: {ex.Message}"));
        }
    }
}