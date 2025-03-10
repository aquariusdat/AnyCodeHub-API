using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseQA;

public static class Command
{
    public record CreateCourseQACommand(string question, string answer) : ICommand<Response.CourseQAResponse> { }
    public record UpdateCourseQACommand(Guid id, string question, string answer) : ICommand<Response.CourseQAResponse> { }
}