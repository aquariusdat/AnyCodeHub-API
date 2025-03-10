namespace AnyCodeHub.Contract.Services.V1.Category;

public static class Response
{
    public record CategoryResponse(Guid id, string name, string? description) { }
}