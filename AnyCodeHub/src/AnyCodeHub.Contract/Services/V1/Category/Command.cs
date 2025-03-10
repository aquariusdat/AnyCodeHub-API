using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.Category;

public static class Command
{
    public record CreateCategoryCommand(string name, string? description, Guid createdBy) : ICommand<Response.CategoryResponse> { }
    public record UpdateCategoryCommand(Guid id, string name, string? description, Guid updatedBy) : ICommand<Response.CategoryResponse> { }
    public record DeleteCategoryCommand(Guid id, Guid deletedBy) : ICommand<bool> { }
}