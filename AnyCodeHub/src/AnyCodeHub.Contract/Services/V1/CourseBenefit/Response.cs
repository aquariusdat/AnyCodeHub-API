namespace AnyCodeHub.Contract.Services.V1.CourseBenefit;

public static class Response
{
    public record CourseBenefitResponse(Guid id, string benefitContent) { }
}