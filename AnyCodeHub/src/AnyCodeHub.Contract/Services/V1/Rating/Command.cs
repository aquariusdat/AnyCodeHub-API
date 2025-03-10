using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.Rating;

public static class Command
{
    public record CreateRatingCommand(Guid courseId, Guid userId, int rate, Guid createdBy) : ICommand<Response.RatingResponse> { }
    public record UpdateRatingCommand(Guid id, Guid courseId, Guid userId, int rate, Guid updatedBy) : ICommand<Response.RatingResponse> { }
    public record DeleteRatingCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}