using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.CourseBenefit;

public static class Command
{
    public record CreateCourseBenefitCommand(string benefitContent, Guid courseId, string description, Guid createdBy) : ICommand<Response.CourseBenefitResponse> { }
    public record UpdateCourseBenefitCommand(Guid id, string benefitContent) : ICommand<Response.CourseBenefitResponse> { }
    public record DeleteCourseBenefitCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}