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

public class UpdateCourseBenefitCommandHandler : ICommandHandler<UpdateCourseBenefitCommand, CourseBenefitResponse>
{
    private readonly ILogger<UpdateCourseBenefitCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.CourseBenefit, Guid> _courseBenefitRepository;
    private readonly IMapper _mapper;

    public UpdateCourseBenefitCommandHandler(
        ILogger<UpdateCourseBenefitCommandHandler> logger,
        IRepositoryBase<Domain.Entities.CourseBenefit, Guid> courseBenefitRepository,
        IMapper mapper)
    {
        _logger = logger;
        _courseBenefitRepository = courseBenefitRepository;
        _mapper = mapper;
    }

    public async Task<Result<CourseBenefitResponse>> Handle(UpdateCourseBenefitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course benefit exists
            var courseBenefit = await _courseBenefitRepository.FindByIdAsync(request.id, cancellationToken);
            if (courseBenefit == null || courseBenefit.IsDeleted)
            {
                return Result.Failure<CourseBenefitResponse>(
                    new Error("CourseBenefit.NotFound", $"Course benefit with ID {request.id} not found."));
            }

            // Update the course benefit
            courseBenefit.Update(
                request.id,
                request.benefitContent);

            // Update in repository
            _courseBenefitRepository.Update(courseBenefit);

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
            _logger.LogError($"Error while updating course benefit: {ex.Message}");
            return Result.Failure<CourseBenefitResponse>(
                new Error("CourseBenefit.UpdateFailed", $"Failed to update course benefit: {ex.Message}"));
        }
    }
}