using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseQA;

public static class Command
{
    public record CreateCourseQACommand(string question, string answer, Guid courseId, Guid createdBy) : ICommand<Response.CourseQAResponse> { }
    public record UpdateCourseQACommand(Guid id, string question, string answer, Guid updatedBy) : ICommand<Response.CourseQAResponse> { }
    public record DeleteCourseQACommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}