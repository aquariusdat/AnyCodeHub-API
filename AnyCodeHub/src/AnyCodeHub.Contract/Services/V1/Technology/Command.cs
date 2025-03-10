using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;

namespace AnyCodeHub.Contract.Services.V1.Technology;

public static class Command
{
    public record CreateTechnologyCommand(string name, string? description) : ICommand<Response.TechnologyResponse> { }
    public record UpdateTechnologyCommand(Guid id, string name, string? description) : ICommand<Response.TechnologyResponse> { }
}