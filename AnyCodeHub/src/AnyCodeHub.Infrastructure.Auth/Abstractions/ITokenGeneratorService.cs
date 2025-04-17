using System.Security.Claims;

namespace AnyCodeHub.Infrastructure.Auth.Abstractions;
public interface ITokenGeneratorService
{
    Task<(string AccessToken, DateTime ExpireTime)> GenerateAccessToken(IEnumerable<Claim> claims);
    Task<(string RefreshToken, DateTime ExpireTime)> GenerateRefreshToken(IEnumerable<Claim> claims);
    Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string token, bool isRefreshToken = false);

}
