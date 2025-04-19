using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Query;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;

namespace AnyCodeHub.Application.Usecases.V1.Queries.CourseRequirement;

public class GetCourseRequirementByIdQueryHandler : IQueryHandler<GetCourseRequirementByIdQuery, CourseRequirementResponse>
{
    private readonly ILogger<GetCourseRequirementByIdQueryHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseRequirement, Guid> _courseRequirementRepository;
    private readonly IMapper _mapper;

    public GetCourseRequirementByIdQueryHandler(
        ILogger<GetCourseRequirementByIdQueryHandler> logger,
        IRepositoryBase<Domain.Entities.CourseRequirement, Guid> courseRequirementRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseRequirementRepository = courseRequirementRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseRequirementResponse>> Handle(GetCourseRequirementByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the course requirement by ID
            var courseRequirement = await _courseRequirementRepository.FindByIdAsync(request.Id, cancellationToken);

            if (courseRequirement == null || courseRequirement.IsDeleted)
            {
                return Result.Failure<CourseRequirementResponse>(
                    new Error("CourseRequirement.NotFound", $"Course requirement with ID {request.Id} not found."));
            }

            // Create response
            var response = new CourseRequirementResponse(
                courseRequirement.Id,
                courseRequirement.RequirementContent,
                courseRequirement.CourseId,
                courseRequirement.CreatedAt,
                courseRequirement.UpdatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting course requirement by ID: {ex.Message}");
            return Result.Failure<CourseRequirementResponse>(
                new Error("CourseRequirement.QueryFailed", $"Failed to query course requirement: {ex.Message}"));
        }
    }
}