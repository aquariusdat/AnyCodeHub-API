namespace AnyCodeHub.Contract.Services.V1.Rating;

public static class Response
{
    public record RatingResponse(Guid id, Guid courseId, Guid userId, int rate) { }
}