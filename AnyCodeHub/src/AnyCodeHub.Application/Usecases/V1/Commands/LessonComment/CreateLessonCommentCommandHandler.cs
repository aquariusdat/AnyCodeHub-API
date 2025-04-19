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

public class CreateLessonCommentCommandHandler : ICommandHandler<CreateLessonCommentCommand, LessonCommentResponse>
{
    private readonly ILogger<CreateLessonCommentCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.LessonComment, Guid> _lessonCommentRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IMapper _mapper;

    public CreateLessonCommentCommandHandler(
        ILogger<CreateLessonCommentCommandHandler> logger,
        IRepositoryBase<Domain.Entities.LessonComment, Guid> lessonCommentRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonCommentRepository = lessonCommentRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<LessonCommentResponse>> Handle(CreateLessonCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the lesson exists
            var lesson = await _lessonRepository.FindByIdAsync(request.lessonId, cancellationToken);
            if (lesson == null || lesson.IsDeleted)
            {
                return Result.Failure<LessonCommentResponse>(
                    new Error("LessonComment.LessonNotFound", $"Lesson with ID {request.lessonId} not found."));
            }

            // Create the lesson comment
            var lessonComment = Domain.Entities.LessonComment.Create(
                request.lessonId,
                request.commentId,
                request.content,
                request.createdBy);

            // Add to repository
            _lessonCommentRepository.Add(lessonComment);

            // Map to response and return
            var response = _mapper.Map<LessonCommentResponse>(lessonComment);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating lesson comment: {ex.Message}");
            return Result.Failure<LessonCommentResponse>(
                new Error("LessonComment.CreateFailed", $"Failed to create lesson comment: {ex.Message}"));
        }
    }
}