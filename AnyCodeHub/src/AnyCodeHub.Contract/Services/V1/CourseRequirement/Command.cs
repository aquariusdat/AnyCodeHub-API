using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseRequirement;

public static class Command
{
    public record CreateCourseRequirementCommand(string requirementContent) : ICommand<Response.CourseRequirementResponse> { }
    public record UpdateCourseRequirementCommand(Guid id, string requirementContent) : ICommand<Response.CourseRequirementResponse> { }
}