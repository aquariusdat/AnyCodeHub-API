using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.CourseBenefit;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Command;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.CourseBenefit;

public class CreateCourseBenefitCommandHandler : ICommandHandler<CreateCourseBenefitCommand, CourseBenefitResponse>
{
    private readonly ILogger<CreateCourseBenefitCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseBenefit, Guid> _courseBenefitRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public CreateCourseBenefitCommandHandler(
        ILogger<CreateCourseBenefitCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseBenefit, Guid> courseBenefitRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseBenefitRepository = courseBenefitRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseBenefitResponse>> Handle(CreateCourseBenefitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<CourseBenefitResponse>(
                    new Error("CourseBenefit.CourseNotFound", $"Course with ID {request.courseId} not found."));
            }

            // Create the course benefit
            var courseBenefit = Domain.Entities.CourseBenefit.Create(
                request.benefitContent,
                request.courseId,
                request.description,
                request.createdBy);

            // Add to repository
            _courseBenefitRepository.Add(courseBenefit);

            // Create response
            var response = new CourseBenefitResponse(
                courseBenefit.Id,
                courseBenefit.BenefitContent,
                courseBenefit.CourseId,
                courseBenefit.Description,
                courseBenefit.CreatedAt);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating course benefit: {ex.Message}");
            return Result.Failure<CourseBenefitResponse>(
                new Error("CourseBenefit.CreateFailed", $"Failed to create course benefit: {ex.Message}"));
        }
    }
}