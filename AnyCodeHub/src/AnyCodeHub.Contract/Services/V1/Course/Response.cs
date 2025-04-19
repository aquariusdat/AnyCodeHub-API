namespace AnyCodeHub.Contract.Services.V1.Course;

public static class Response
{
    public record CourseResponse(
        Guid id,
        string name,
        string? description,
        string? thumbnailUrl,
        decimal price,
        Guid instructorId,
        string instructorName,
        string instructorAvatarUrl,
        double averageRating,
        int totalStudents,
        int totalLessons,
        int totalSections,
        DateTime createdAt,
        DateTime? updatedAt);

    public record CourseDetailResponse(
        Guid id,
        string name,
        string? description,
        string? thumbnailUrl,
        decimal price,
        Guid instructorId,
        string instructorName,
        string instructorAvatarUrl,
        double averageRating,
        int totalStudents,
        int totalLessons,
        int totalSections,
        List<SectionInfo> sections,
        List<RequirementInfo> requirements,
        List<BenefitInfo> benefits,
        List<QAInfo> qaList,
        List<TechnologyInfo> technologies,
        List<CategoryInfo> categories,
        DateTime createdAt,
        DateTime? updatedAt);

    public record SectionInfo(
        Guid id,
        string name,
        List<LessonInfo> lessons);

    public record LessonInfo(
        Guid id,
        string title,
        string? description,
        string? videoUrl,
        string? pdfUrl,
        double duration);

    public record RequirementInfo(
        Guid id,
        string requirementContent);

    public record BenefitInfo(
        Guid id,
        string benefitContent,
        string description);

    public record QAInfo(
        Guid id,
        string question,
        string answer);

    public record TechnologyInfo(
        Guid id,
        string name,
        string description);

    public record CategoryInfo(
        Guid id,
        string name,
        string? description);
}
