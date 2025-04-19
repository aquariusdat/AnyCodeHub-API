using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Section;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Section.Command;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Section;

public class CreateSectionCommandHandler : ICommandHandler<CreateSectionCommand, SectionResponse>
{
    private readonly ILogger<CreateSectionCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _courseRepository;
    private readonly IMapper _mapper;

    public CreateSectionCommandHandler(
        ILogger<CreateSectionCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.Course, Guid> courseRepository,
        IMapper mapper)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<Result<SectionResponse>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the course exists
            var course = await _courseRepository.FindByIdAsync(request.courseId, cancellationToken);
            if (course == null || course.IsDeleted)
            {
                return Result.Failure<SectionResponse>(
                    new Error("Course.NotFound", $"Course with ID {request.courseId} not found."));
            }

            // Create new section
            var section = Domain.Entities.Section.Create(
                request.name,
                request.courseId,
                request.createdBy);

            // Add to repository
            _sectionRepository.Add(section);

            // Map to response
            var response = _mapper.Map<SectionResponse>(section);

            // Since the mapper might not map course name, we'll set it directly
            response = response with { CourseName = course.Name };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating section: {ex.Message}");
            return Result.Failure<SectionResponse>(
                new Error("Section.CreateFailed", $"Failed to create section: {ex.Message}"));
        }
    }
}