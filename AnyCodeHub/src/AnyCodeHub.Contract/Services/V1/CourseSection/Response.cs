namespace AnyCodeHub.Contract.Services.V1.CourseSection;

public static class Response
{
    public record CourseSectionResponse(Guid id, Guid courseId, Guid lectureId) { }
}