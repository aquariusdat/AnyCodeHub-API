namespace AnyCodeHub.Contract.Services.V1.SectionLesson;

public static class Response
{
    public record SectionLessonResponse(
        Guid Id,
        Guid SectionId,
        string SectionName,
        Guid LessonId,
        string LessonTitle,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record SectionLessonDetailResponse(
        Guid Id,
        Guid SectionId,
        string SectionName,
        Guid CourseId,
        string CourseName,
        Guid LessonId,
        string LessonTitle,
        string? LessonDescription,
        string? VideoUrl,
        string? PdfUrl,
        double Duration,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}