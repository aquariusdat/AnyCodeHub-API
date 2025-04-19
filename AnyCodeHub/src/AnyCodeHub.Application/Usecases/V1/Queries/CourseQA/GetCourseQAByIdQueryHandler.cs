using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Query;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseQA;

public class GetCourseQAByIdQueryHandler : IQueryHandler<GetCourseQAByIdQuery, CourseQAResponse>
{
    private readonly ILogger<GetCourseQAByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseQA, Guid> _courseQARepository;
    private readonly IMapper _mapper;

    public GetCourseQAByIdQueryHandler(
        ILogger<GetCourseQAByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseQA, Guid> courseQARepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseQARepository = courseQARepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseQAResponse>> Handle(GetCourseQAByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the course QA by ID
            var courseQA = await _courseQARepository.FindByIdAsync(request.Id, cancellationToken);

            if (courseQA == null || courseQA.IsDeleted)
            {
                return Result.Failure<CourseQAResponse>(
                    new Error("CourseQA.NotFound", $"Course QA with ID {request.Id} not found."));
            }

            // Create response
            var response = new CourseQAResponse(
                courseQA.Id,
                courseQA.Question,
                courseQA.Answer,
                courseQA.CourseId,
                courseQA.CreatedAt,
                courseQA.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course QA by ID: {ex.Message}");
            return Result.Failure<CourseQAResponse>(
                new Error("CourseQA.QueryFailed", $"Failed to query course QA: {ex.Message}"));
        }
    }
}