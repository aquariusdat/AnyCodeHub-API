using AnyCodeHub.Contract.Abstractions.Message;
using static AnyCodeHub.Contract.Services.V1.Section.Response;

namespace AnyCodeHub.Contract.Services.V1.Section;

public static class Command
{
    public record CreateSectionCommand(
        string name,
        Guid courseId,
        Guid createdBy) : ICommand<SectionResponse>;

    public record UpdateSectionCommand(
        Guid id,
        string name,
        Guid courseId,
        Guid updatedBy) : ICommand<SectionResponse>;

    public record DeleteSectionCommand(
        Guid id,
        Guid deletedBy) : ICommand<bool>;
}