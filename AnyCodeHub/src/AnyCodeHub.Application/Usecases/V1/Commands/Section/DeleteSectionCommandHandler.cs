using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Contract.Services.V1.Section.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Section;

public class DeleteSectionCommandHandler : ICommandHandler<DeleteSectionCommand, bool>
{
    private readonly ILogger<DeleteSectionCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Section, Guid> _sectionRepository;
    private readonly IRepositoryBase<Domain.Entities.SectionLesson, Guid> _sectionLessonRepository;

    public DeleteSectionCommandHandler(
        ILogger<DeleteSectionCommandHandler> logger,
        IRepositoryBase<Domain.Entities.Section, Guid> sectionRepository,
        IRepositoryBase<Domain.Entities.SectionLesson, Guid> sectionLessonRepository)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _sectionLessonRepository = sectionLessonRepository;
    }

    public async Task<Result<bool>> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify that the section exists
            var section = await _sectionRepository.FindByIdAsync(request.id, cancellationToken);
            if (section == null)
            {
                return Result.Failure<bool>(
                    new Error("Section.NotFound", $"Section with ID {request.id} not found."));
            }

            // Check if there are any associated SectionLessons
            var sectionLessons = _sectionLessonRepository.FindAll(sl => sl.SectionId == request.id).ToList();

            // Mark the section as deleted
            section.Delete(request.deletedBy);

            // Update the section in the repository
            _sectionRepository.Update(section);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting section: {ex.Message}");
            return Result.Failure<bool>(
                new Error("Section.DeleteFailed", $"Failed to delete section: {ex.Message}"));
        }
    }
}