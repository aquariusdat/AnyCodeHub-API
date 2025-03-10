namespace AnyCodeHub.Contract.Services.V1.CourseRequirement;

public static class Response
{
    public record CourseRequirementResponse(Guid id, string requirementContent) { }
}