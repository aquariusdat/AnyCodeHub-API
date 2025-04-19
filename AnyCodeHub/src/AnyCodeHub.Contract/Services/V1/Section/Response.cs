namespace AnyCodeHub.Contract.Services.V1.Section;

public static class Response
{
    public record SectionResponse(
        Guid Id,
        string Name,
        Guid CourseId,
        string CourseName,
        int LessonCount,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record SectionDetailResponse(
        Guid Id,
        string Name,
        Guid CourseId,
        string CourseName,
        List<LessonInfo> Lessons,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record LessonInfo(
        Guid Id,
        string Title,
        string? Description,
        string? VideoUrl,
        string? PdfUrl,
        double Duration);
}