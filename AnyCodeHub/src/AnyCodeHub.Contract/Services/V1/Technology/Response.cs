namespace AnyCodeHub.Contract.Services.V1.Technology;

public static class Response
{
    public record TechnologyResponse(Guid id, string name, string? description) { }
}