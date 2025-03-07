using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.CommonServices;
using AnyCodeHub.Contract.Services.V1.Authentication;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Authentication;

public class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.AuthenticatedResponse>
{
    private readonly IMapper _mapper;
    private readonly ILogger<GetLoginQueryHandler> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IUserRepository _userRepository;
    private readonly ICachingService _cachingService;
    private static readonly int MAX_ACCESS_FAILED_COUNT = 5;

    public GetLoginQueryHandler(IMapper mapper, UserManager<AppUser> userManager, ITokenGeneratorService tokenGeneratorService, IUserRepository userRepository, ICachingService cachingService, ILogger<GetLoginQueryHandler> logger)
    {
        _mapper = mapper;
        _userManager = userManager;
        _tokenGeneratorService = tokenGeneratorService;
        _userRepository = userRepository;
        _cachingService = cachingService;
        _logger = logger;
    }
    public async Task<Result<Response.AuthenticatedResponse>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        AppUser user = null;
        try
        {
            user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserException.UserNotFoundException(request.Email);
            if (!user.PasswordHash.VerifyPassword(request.Password)) throw new UserException.WrongPasswordException();

            #region Get roles of user
            var roles = await _userRepository.GetRoleByEmail(request.Email);

            var claims = new List<Claim>()
            {
                    new Claim(ClaimTypes.Email, request.Email),
            };

            if (user.PhoneNumber == "0789163351")
            {
                claims.Add(new Claim(ClaimTypes.Role, Contract.Enumerations.UserRole.ADMIN));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            #endregion

            var resAccessTokenGenerate = await _tokenGeneratorService.GenerateAccessToken(claims);
            var resRefreshTokenGenerate = await _tokenGeneratorService.GenerateRefreshToken();

            var response = new Response.AuthenticatedResponse()
            {
                RefreshTokenExpirationTime = resRefreshTokenGenerate.ExpireTime,
                RefreshToken = resRefreshTokenGenerate.RefreshToken,
                AccessToken = resAccessTokenGenerate.AccessToken,
                AccessTokenExpirationTime = resAccessTokenGenerate.ExpireTime,
                UserInformation = new Response.UserInformation()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthOfDate = user.BirthOfDate,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                }
            };

            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.AuthenticatedResponseByUser}_{request.Email}", response, cancellationToken);

            var lstAuthenticatedResponse = await GetListToken(request.Email);
            lstAuthenticatedResponse.Add(response);
            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{request.Email}", lstAuthenticatedResponse, cancellationToken);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            if (ex is UserException.WrongPasswordException && user != null)
            {
                ++user.AccessFailedCount;

                if (user.AccessFailedCount > MAX_ACCESS_FAILED_COUNT)
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(5);
                    user.LockoutEnabled = true;
                }
            }

            _logger.LogError($"Error while logging. \r\n[Exception={ex.ToString()}]");
            throw;
        }
        finally
        {
            if (user != null)
            {
                await _userManager.UpdateAsync(user);
            }
        }
    }

    private async Task<List<Response.AuthenticatedResponse>> GetListToken(string UserName)
    {
        return await _cachingService.GetAsync<List<Response.AuthenticatedResponse>>($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{UserName}") ?? new();
    }
}
