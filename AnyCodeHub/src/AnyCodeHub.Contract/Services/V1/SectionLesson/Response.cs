namespace AnyCodeHub.Contract.Services.V1.SectionLesson;

public static class Response
{
    public record SectionLessonResponse(Guid id, Guid sectionId, Guid lessonId) { }
}