using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseSection;

public static class Command
{
    public record CreateCourseSectionCommand(Guid courseId, Guid lectureId, Guid createdBy) : ICommand<Response.CourseSectionResponse> { }
    public record UpdateCourseSectionCommand(Guid id, Guid courseId, Guid lectureId, Guid updatedBy) : ICommand<Response.CourseSectionResponse> { }
    public record DeleteCourseSectionCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}