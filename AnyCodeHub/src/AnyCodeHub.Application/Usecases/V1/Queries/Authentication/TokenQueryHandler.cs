using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.Authentication;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static AnyCodeHub.Domain.Exceptions.IdentityException;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AnyCodeHub.Contract.CommonServices;
using Microsoft.Extensions.Caching.Distributed;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Contract.Abstractions.Services;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Authentication;

public class TokenQueryHandler : IQueryHandler<Query.Token, Response.AuthenticatedResponse>
{
    private readonly ILogger<TokenQueryHandler> _logger;
    private readonly ICachingService _cachingService;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IEmailSenderService _emailSenderService;
    private static readonly int MAX_TRYCOUNT_ACCESSTOKEN_NOTEXPIREYET = 3;
    private static readonly int MINITUES_TO_CHECK_SPAM_TOKEN = 3;

    public TokenQueryHandler(ILogger<TokenQueryHandler> logger, ICachingService cachingService , ITokenGeneratorService tokenGeneratorService, UserManager<AppUser> userManager, IUserRepository userRepository, IEmailSenderService emailSenderService)
    {
        _logger = logger;
        _cachingService = cachingService;
        _tokenGeneratorService = tokenGeneratorService;
        _userManager = userManager;
        _userRepository = userRepository;
        _emailSenderService = emailSenderService;
    }

    public async Task<Result<Response.AuthenticatedResponse>> Handle(Query.Token request, CancellationToken cancellationToken)
    {
        string email = string.Empty;
        try
        {
            // Get principal from token
            var principal = await _tokenGeneratorService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal is null || !principal.Claims.Any(t => t.Type == ClaimTypes.Email))
            {
                throw new TokenException("Invalid access token.");
            }

            email = principal.FindFirstValue(JwtRegisteredClaimNames.Email) ?? principal.FindFirstValue(ClaimTypes.Email);

            // Check invalid refresh token to generate new token??
            if (await IsInvalidGenerateTokenRequest(request.RefreshToken, email))
                throw new TokenException($"Invalid refresh token.");

            // Check spam generating token
            if (await CheckSpamGenerateNewToken(request.RefreshToken, email, principal))
                throw new TokenException($"Because you are requesting to generate the new access token too many times while it hasn't expired yet. So, you need to log in again.");

            // Check user
            var loginUser = await _userRepository.GetByEmail(email);

            // Generate Jwt Token
            var resAccessTokenGenerate = await _tokenGeneratorService.GenerateAccessToken(principal.Claims);
            var resRefreshTokenGenerate = await _tokenGeneratorService.GenerateRefreshToken();

            var response = new Response.AuthenticatedResponse()
            {
                RefreshTokenExpirationTime = resRefreshTokenGenerate.ExpireTime,
                RefreshToken = resRefreshTokenGenerate.RefreshToken,
                AccessToken = resAccessTokenGenerate.AccessToken,
                AccessTokenExpirationTime = resAccessTokenGenerate.ExpireTime,
                UserInformation = new Response.UserInformation()
                {
                    FirstName = loginUser.FirstName,
                    LastName = loginUser.LastName,
                    BirthOfDate = loginUser.BirthOfDate,
                    Email = loginUser.Email,
                }
            };

            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.AuthenticatedResponseByUser}_{email}", response, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

            var lstAuthenticatedResponse = await GetListToken(email);
            lstAuthenticatedResponse.Add(response);
            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{email}", lstAuthenticatedResponse, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

            return Result.Success(response);
        }
        catch(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
            {
                _cachingService.RemoveAsync($"{Contract.Enumerations.CachingPrefixKey.AuthenticatedResponseByUser}_{email}", new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            }

            _logger.LogError($"Error while creating new token.");
            throw;
        }
    }
    /// <summary>
    /// Case: Requesting generate new token but token hasn't expired yet.
    /// </summary>
    /// <param name="RefreshToken"></param>
    /// <param name="UserName"></param>
    /// <param name="principal"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    private async Task<bool> CheckSpamGenerateNewToken(string RefreshToken, string Email, ClaimsPrincipal principal)
    {
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        string keyCache = $"TOKEN_NOTEXPIREYET_{Email}_{RefreshToken}";

        long utcExpireDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expireDate = CommonHelper.ConvertUnixTimeToDateTime(utcExpireDate);

        // If request generate new access token while access token doesn't expire over 5 times then must be login again.
        if (expireDate > DateTime.Now)
        {
            var cacheValue = await _cachingService.GetAsync<string>(keyCache, cancellationTokenSource.Token);
            if (string.IsNullOrEmpty(cacheValue))
            {
                cacheValue = "0";
            }
            if (int.TryParse(cacheValue, out int count))
            {
                await _cachingService.SetAsync(keyCache, (++count).ToString(), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(MINITUES_TO_CHECK_SPAM_TOKEN),
                }, cancellationTokenSource.Token);
            }

            bool isSpam = count >= MAX_TRYCOUNT_ACCESSTOKEN_NOTEXPIREYET;

            if (isSpam)
                await _cachingService.RemoveAsync(keyCache, cancellationTokenSource.Token); // Refresh cached data

            _emailSenderService.SendAsync(Email, "[WARNING] Should your account is hacked?", "", $"Your account are spamming to generate token. Is it you? If it isn't you then let me revoke your token <a href=\"www.google.com\">here</a>");

            return isSpam;
        }

        await _cachingService.SetAsync(keyCache, "0", new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(MINITUES_TO_CHECK_SPAM_TOKEN),
        }, cancellationTokenSource.Token);
        return false;
    }

    private async Task<List<Response.AuthenticatedResponse>> GetListToken(string UserName)
    {
        return await _cachingService.GetAsync<List<Response.AuthenticatedResponse>>($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{UserName}");
    }
    private async Task<bool> IsInvalidGenerateTokenRequest(string RefreshToken, string Email)
    {

        var authenticatedResponse = await _cachingService.GetAsync<Response.AuthenticatedResponse>($"{Contract.Enumerations.CachingPrefixKey.AuthenticatedResponseByUser}_{Email}", new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token) ?? throw new TokenException($"Please login again.");

        return authenticatedResponse?.RefreshToken != RefreshToken || authenticatedResponse.RefreshTokenExpirationTime <= DateTime.Now;
    }
}
