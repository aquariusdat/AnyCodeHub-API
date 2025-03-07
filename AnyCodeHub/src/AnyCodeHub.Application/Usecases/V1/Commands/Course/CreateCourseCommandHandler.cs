
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Course;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Course;

public class CreateCourseCommandHandler : ICommandHandler<Command.CreateCourseCommand, Response.CourseResponse>
{
    private readonly ILogger<CreateCourseCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _repository;
    public CreateCourseCommandHandler(ILogger<CreateCourseCommandHandler> logger, IRepositoryBase<Domain.Entities.Course, Guid> repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public Task<Result<Response.CourseResponse>> Handle(Command.CreateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var n
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error while creating course. [Error={ex.ToString()}]");
            throw;
        }
    }
}
