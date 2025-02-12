using System.Security.Claims;
using AnyCodeHub.Domain.ValueObjects;

namespace AnyCodeHub.Infrastructure.Auth.Abstractions.Auth;
public interface IAuthenticationService
{
    Task<AuthToken> GetNewTokenByRefreshToken(AuthToken AuthToken);
}
