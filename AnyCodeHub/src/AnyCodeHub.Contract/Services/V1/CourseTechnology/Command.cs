using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseTechnology;

public static class Command
{
    public record CreateCourseTechnologyCommand(Guid courseId, Guid technologyId, Guid createdBy) : ICommand<Response.CourseTechnologyResponse> { }
    public record UpdateCourseTechnologyCommand(Guid id, Guid courseId, Guid technologyId, Guid updatedBy) : ICommand<Response.CourseTechnologyResponse> { }
    public record DeleteCourseTechnologyCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}