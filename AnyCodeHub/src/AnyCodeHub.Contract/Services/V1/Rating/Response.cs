namespace AnyCodeHub.Contract.Services.V1.Rating;

public static class Response
{
    public record RatingResponse(
        Guid id,
        Guid courseId,
        string courseName,
        Guid userId,
        string userName,
        int rate,
        DateTime createdAt,
        DateTime? updatedAt);
}