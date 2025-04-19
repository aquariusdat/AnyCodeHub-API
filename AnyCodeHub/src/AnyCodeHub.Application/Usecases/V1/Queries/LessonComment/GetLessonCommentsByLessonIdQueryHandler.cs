using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Query;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.LessonComment;

public class GetLessonCommentsByLessonIdQueryHandler : IQueryHandler<GetLessonCommentsByLessonIdQuery, PagedResult<LessonCommentResponse>>
{
    private readonly ILogger<GetLessonCommentsByLessonIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.LessonComment, Guid> _lessonCommentRepository;
    private readonly IRepositoryBase<Domain.Entities.Lesson, Guid> _lessonRepository;
    private readonly IMapper _mapper;

    public GetLessonCommentsByLessonIdQueryHandler(
        ILogger<GetLessonCommentsByLessonIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.LessonComment, Guid> lessonCommentRepository,
        IRepositoryBase<Domain.Entities.Lesson, Guid> lessonRepository,
        IMapper mapper)
    {
        _logger = logger;
        _lessonCommentRepository = lessonCommentRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<LessonCommentResponse>>> Handle(GetLessonCommentsByLessonIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the lesson exists
            var lesson = await _lessonRepository.FindByIdAsync(request.LessonId, cancellationToken);
            if (lesson == null || lesson.IsDeleted)
            {
                return Result.Failure<PagedResult<LessonCommentResponse>>(
                    new Error("LessonComment.LessonNotFound", $"Lesson with ID {request.LessonId} not found."));
            }

            // Build the query
            IQueryable<Domain.Entities.LessonComment> query = _lessonCommentRepository.FindAll(lc =>
                lc.LessonId == request.LessonId && !lc.IsDeleted);

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string searchTerm = request.SearchTerm.ToLower();
                query = query.Where(lc => lc.Content.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn) && request.SortOrder != null)
            {
                query = ApplySorting(query, request.SortColumn, request.SortOrder);
            }
            else
            {
                // Default sorting by created date descending (newest first)
                query = query.OrderByDescending(lc => lc.CreatedAt);
            }

            // Calculate total count for pagination
            int totalCount = query.Count();

            // Apply pagination
            var pagedItems = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to response DTOs
            var commentResponses = _mapper.Map<List<LessonCommentResponse>>(pagedItems);

            // Create paged result
            var pagedResult = PagedResult<LessonCommentResponse>.Create(
                commentResponses,
                request.PageIndex,
                request.PageSize,
                totalCount);

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting lesson comments: {ex.Message}");
            return Result.Failure<PagedResult<LessonCommentResponse>>(
                new Error("LessonComment.QueryFailed", $"Failed to query lesson comments: {ex.Message}"));
        }
    }

    private IQueryable<Domain.Entities.LessonComment> ApplySorting(
        IQueryable<Domain.Entities.LessonComment> query,
        string sortColumn,
        SortOrder sortOrder)
    {
        Expression<Func<Domain.Entities.LessonComment, object>> keySelector = sortColumn.ToLower() switch
        {
            "content" => lc => lc.Content,
            "createdat" => lc => lc.CreatedAt,
            "commentid" => lc => lc.CommentId,
            _ => lc => lc.CreatedAt // Default sort by created date
        };

        return sortOrder == SortOrder.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
}