using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Section;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Section.Command;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Section;

public class UpdateSectionCommandHandler : ICommandHandler<UpdateSectionCommand, SectionResponse>
{
    private readonly ILogger<UpdateSectionCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public UpdateSectionCommandHandler(
        ILogger<UpdateSectionCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<SectionResponse>> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the section exists
            var section = await _sectionRepository.FindByIdAsync(request.id, cancellationToken);
            if (section == null)
            {
                return Result.Failure<SectionResponse>(
                    new Error("Section.NotFound", $"Section with ID {request.id} not found."));
            }

            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null)
            {
                return Result.Failure<SectionResponse>(
                    new Error("Section.CourseNotFound", $"Course with ID {request.courseId} not found."));
            }

            // Update the section
            section.Update(
                request.id,
                request.name,
                request.courseId,
                request.updatedBy);

            // Save changes to repository
            _sectionRepository.Update(section);

            // Map to response and return
            var response = _mapper.Map<SectionResponse>(section);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating section: {ex.Message}");
            return Result.Failure<SectionResponse>(
                new Error("Section.UpdateFailed", $"Failed to update section: {ex.Message}"));
        }
    }
}