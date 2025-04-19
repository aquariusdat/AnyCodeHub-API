using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.SectionLesson;

public static class Command
{
    public record CreateSectionLessonCommand(
        Guid sectionId,
        Guid lessonId,
        Guid createdBy) : ICommand<Response.SectionLessonResponse>;

    public record UpdateSectionLessonCommand(
        Guid id,
        Guid sectionId,
        Guid lessonId,
        Guid updatedBy) : ICommand<Response.SectionLessonResponse>;

    public record DeleteSectionLessonCommand(
        Guid id,
        Guid deletedBy) : ICommand<bool>;
}