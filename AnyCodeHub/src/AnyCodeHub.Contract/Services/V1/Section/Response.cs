namespace AnyCodeHub.Contract.Services.V1.Section;

public static class Response
{
    public record SectionResponse(Guid id, string name, Guid courseId) { }
}