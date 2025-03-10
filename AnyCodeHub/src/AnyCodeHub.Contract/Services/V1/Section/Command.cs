using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.Section;

public static class Command
{
    public record CreateSectionCommand(string name, Guid courseId, Guid createdBy) : ICommand<Response.SectionResponse> { }
    public record UpdateSectionCommand(Guid id, string name, Guid courseId, Guid updatedBy) : ICommand<Response.SectionResponse> { }
    public record DeleteSectionCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}