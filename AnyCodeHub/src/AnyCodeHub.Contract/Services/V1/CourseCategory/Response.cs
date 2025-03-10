namespace AnyCodeHub.Contract.Services.V1.CourseCategory;

public static class Response
{
    public record CourseCategoryResponse(Guid id, Guid courseId, Guid categoryId) { }
}