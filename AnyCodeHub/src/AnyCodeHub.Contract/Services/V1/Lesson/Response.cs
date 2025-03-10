namespace AnyCodeHub.Contract.Services.V1.Lesson;

public static class Response
{
    public record LessonResponse(Guid id, string title, string? description, string? videoUrl, string? pdfUrl, Guid sectionId, Guid? courseId, double duration) { }
}