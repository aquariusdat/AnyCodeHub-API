namespace AnyCodeHub.Contract.Services.V1.CourseQA;

public static class Response
{
    public record CourseQAResponse(Guid id, string question, string answer) { }
}