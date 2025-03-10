namespace AnyCodeHub.Contract.Services.V1.CourseTechnology;

public static class Response
{
    public record CourseTechnologyResponse(Guid id, Guid courseId, Guid technologyId) { }
}