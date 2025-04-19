using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Lesson.Query;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Lesson;

public class GetLessonByIdQueryHandler : IQueryHandler<GetLessonByIdQuery, LessonResponse>
{
    private readonly ILogger<GetLessonByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IMapper _mapper;

    public GetLessonByIdQueryHandler(
        ILogger<GetLessonByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<LessonResponse>> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the lesson by ID
            var lesson = await _lessonRepository.FindByIdAsync(request.Id, cancellationToken);

            if (lesson == null || lesson.IsDeleted)
            {
                return Result.Failure<LessonResponse>(
                    new Error("Lesson.NotFound", $"Lesson with ID {request.Id} not found."));
            }

            // Map to response DTO
            var lessonResponse = _mapper.Map<LessonResponse>(lesson);

            return Result.Success(lessonResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting lesson by ID: {ex.Message}");
            return Result.Failure<LessonResponse>(
                new Error("Lesson.QueryFailed", $"Failed to query lesson: {ex.Message}"));
        }
    }
}