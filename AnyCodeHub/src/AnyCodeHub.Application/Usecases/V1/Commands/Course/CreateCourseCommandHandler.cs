
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Course;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Course;

public class CreateCourseCommandHandler : ICommandHandler<Command.CreateCourseCommand, Response.CourseResponse>
{
    private readonly ILogger<CreateCourseCommandHandler> _logger;
    private readonly IRepositoryBase<Domain.Entities.Course, Guid> _repository;
    private readonly IMapper _mapper;
    public CreateCourseCommandHandler(ILogger<CreateCourseCommandHandler> logger, IRepositoryBase<Domain.Entities.Course, Guid> repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<Result<Response.CourseResponse>> Handle(Command.CreateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return Result.Success(_mapper.Map<Response.CourseResponse>(null));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while creating course. [Error={ex.ToString()}]");
            throw;
        }
    }
}
