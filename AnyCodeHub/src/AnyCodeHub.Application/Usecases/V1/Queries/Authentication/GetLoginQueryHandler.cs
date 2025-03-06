using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Authentication;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Authentication;

public class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.AuthenticatedResponse>
{
    private readonly IMapper _mapper;
    private readonly ILogger<GetLoginQueryHandler> _logger;
    private readonly UserManager<AppUser> _userManager;
    public GetLoginQueryHandler(IMapper mapper, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    public Task<Result<Response.AuthenticatedResponse>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        try
        {
            var user = _userManager.FindByEmailAsync(request.Email) ?? throw new UserException.UserNotFoundException(request.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while logging. \r\n[Exception={ex.ToString()}]");
            throw;
        }
    }
}
