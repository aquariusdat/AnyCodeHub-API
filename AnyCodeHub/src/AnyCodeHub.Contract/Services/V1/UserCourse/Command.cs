using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.UserCourse;

public static class Command
{
    public record CreateUserCourseCommand(Guid userId, Guid courseId, Guid createdBy) : ICommand<Response.UserCourseResponse> { }
    public record UpdateUserCourseCommand(Guid id, Guid userId, Guid courseId, Guid updatedBy) : ICommand<Response.UserCourseResponse> { }
    public record DeleteUserCourseCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}