namespace AnyCodeHub.Contract.Services.V1.Course;

public static class Response
{
    public record CourseResponse(Guid id, string name, string? description, decimal price, decimal? salePrice, string? imageUrl, string? videoUrl, string? slug, string status, Guid authorId, int level, int totalViews, double totalDuration, double rating) { }
}
