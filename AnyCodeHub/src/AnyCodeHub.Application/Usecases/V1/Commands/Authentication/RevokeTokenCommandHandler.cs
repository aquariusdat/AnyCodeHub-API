using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Authentication;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Domain.Exceptions.IdentityException;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Authentication;

public class RevokeTokenCommandHandler : ICommandHandler<Command.RevokeTokenCommand, bool>
{
    private readonly ICachingService _cachingService;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly ILogger<RevokeTokenCommandHandler> _logger;
    public RevokeTokenCommandHandler(ITokenGeneratorService tokenGeneratorService, ICachingService cachingService, ILogger<RevokeTokenCommandHandler> logger)
    {
        _tokenGeneratorService = tokenGeneratorService;
        _cachingService = cachingService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(Command.RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var principal = await _tokenGeneratorService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal is null || !principal.Claims.Any(t => t.Type == ClaimTypes.Email || t.Type == JwtRegisteredClaimNames.Email)) throw new TokenException("Invalid access token.");

            string email = principal.FindFirstValue(ClaimTypes.Email) ?? principal.FindFirstValue(JwtRegisteredClaimNames.Email);

            //var authenticated = await _cachingService.GetAsync<List<Response.AuthenticatedResponse>>($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{email}", cancellationToken) 
            //    ?? throw new Exception("Cannot get access token from redis");

            _cachingService.RemoveAsync($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{email}", cancellationToken);

            return Result.Success(true);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error while revoking token. [Error={ex.ToString()}]");
            throw;
        }
    }
}
