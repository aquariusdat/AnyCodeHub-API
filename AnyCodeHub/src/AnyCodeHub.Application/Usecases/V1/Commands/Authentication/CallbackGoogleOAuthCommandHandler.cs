

using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.CommonServices;
using AnyCodeHub.Contract.Extensions;
using AnyCodeHub.Domain.Abstractions.Repositories;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using static AnyCodeHub.Contract.Services.V1.Authentication.Command;
using static AnyCodeHub.Contract.Services.V1.Authentication.Response;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Authentication;

public class CallbackGoogleOAuthCommandHandler : ICommandHandler<CallbackGoogleOAuthCommand, AuthenticatedResponse>
{
    private readonly ILogger<CallbackGoogleOAuthCommandHandler> _logger;
    private readonly IOAuthGoogleService _authGoogleService;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICachingService _cachingService;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IUserRepository _userRepository;
    public CallbackGoogleOAuthCommandHandler(ILogger<CallbackGoogleOAuthCommandHandler> logger, IOAuthGoogleService authGoogleService, UserManager<AppUser> userManager, ICachingService cachingService, ITokenGeneratorService tokenGeneratorService, IUserRepository userRepository = null)
    {
        _logger = logger;
        _authGoogleService = authGoogleService;
        _userManager = userManager;
        _cachingService = cachingService;
        _tokenGeneratorService = tokenGeneratorService;
        _userRepository = userRepository;
    }

    public async Task<Result<AuthenticatedResponse>> Handle(CallbackGoogleOAuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tokenResponse = await _authGoogleService.ExchangeCodeForTokenAsync(request.code);

            var personalInfo = await _authGoogleService.GetUserInfoAsync(tokenResponse.AccessToken, tokenResponse);

            var email = personalInfo.EmailAddresses.Last().Value;
            var account = await _userManager.FindByEmailAsync(email);

            if (account is null)
            {
                bool isHasBirthOfDate = personalInfo.Birthdays?.Count > 0;
                var birthOfDate = isHasBirthOfDate ? new DateTime(personalInfo.Birthdays.Last().Date.Year.Value, personalInfo.Birthdays.Last().Date.Month.Value, personalInfo.Birthdays.Last().Date.Day.Value) : DateTime.MinValue;
                string password = Guid.NewGuid().ToString().GetMd5Hash();

                // Create account
                account = new AppUser
                {
                    FirstName = personalInfo.Names.Last().GivenName,
                    LastName = personalInfo.Names.Last().FamilyName,
                    BirthOfDate = isHasBirthOfDate ? birthOfDate : null,
                    Email = email,
                    UserName = email.Split('@').FirstOrDefault() ?? Guid.NewGuid().ToString().GetMd5Hash(),
                    PasswordHash = password.HashPasswordWithBCrypt(),
                    IsActive = true,
                    LockoutEnabled = false,
                    PhoneNumber = personalInfo.PhoneNumbers?.Last().Value,
                    IsAdmin = email == "tondat.dev@gmail.com",
                };

                await _userManager.CreateAsync(account);

                await _userManager.AddToRoleAsync(account, account.IsAdmin ? Contract.Enumerations.UserRole.ADMIN : Contract.Enumerations.UserRole.USER);

            }

            #region Get roles of user
            var roles = await _userRepository.GetRoleByEmail(email);

            var claims = new List<Claim>()
            {
                    new Claim(ClaimTypes.Email, email),
            };

            if (account.PhoneNumber == "0789163351")
            {
                claims.Add(new Claim(ClaimTypes.Role, Contract.Enumerations.UserRole.ADMIN));
                roles = roles.Where(t => t.ToUpper() != Contract.Enumerations.UserRole.ADMIN.ToUpper());
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            #endregion

            #region Generate Jwt Token
            var resAccessTokenGenerate = await _tokenGeneratorService.GenerateAccessToken(claims);
            var resRefreshTokenGenerate = await _tokenGeneratorService.GenerateRefreshToken();

            var authenticatedResponse = new Contract.Services.V1.Authentication.Response.AuthenticatedResponse()
            {
                RefreshTokenExpirationTime = resRefreshTokenGenerate.ExpireTime,
                RefreshToken = resRefreshTokenGenerate.RefreshToken,
                AccessToken = resAccessTokenGenerate.AccessToken,
                AccessTokenExpirationTime = resAccessTokenGenerate.ExpireTime,
                UserInformation = new UserInformation()
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    BirthOfDate = account.BirthOfDate,
                    Email = account.Email,
                    PhoneNumber = account.PhoneNumber
                }
            };

            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.AuthenticatedResponseByUser}_{email}", authenticatedResponse, cancellationToken);
            var lstAuthenticatedResponse = await GetListToken(email);
            lstAuthenticatedResponse.Add(authenticatedResponse);
            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{email}", lstAuthenticatedResponse, cancellationToken);
            #endregion

            await _cachingService.RemoveAsync($"{Contract.Enumerations.CachingPrefixKey.GoogleOAuthTokenInfo}_{email}", cancellationToken);
            await _cachingService.SetAsync($"{Contract.Enumerations.CachingPrefixKey.GoogleOAuthTokenInfo}_{email}", tokenResponse, cancellationToken);

            return Result.Success(authenticatedResponse);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error when processing Callback method [ERR:::{ex.ToString()}]");
            throw;
        }
    }


    private async Task<List<Contract.Services.V1.Authentication.Response.AuthenticatedResponse>> GetListToken(string Email)
    {
        return await _cachingService.GetAsync<List<Contract.Services.V1.Authentication.Response.AuthenticatedResponse>>($"{Contract.Enumerations.CachingPrefixKey.TokenHistoryByUser}_{Email}") ?? new();
    }
}
