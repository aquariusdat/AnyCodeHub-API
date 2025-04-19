namespace AnyCodeHub.Contract.Services.V1.CourseCategory;

public static class Response
{
    public record CourseCategoryResponse(
        Guid Id,
        Guid CourseId,
        string CourseName,
        Guid CategoryId,
        string CategoryName,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    public record CourseCategoryDetailResponse(
        Guid Id,
        Guid CourseId,
        string CourseName,
        string CourseDescription,
        Guid CategoryId,
        string CategoryName,
        string CategoryDescription,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}