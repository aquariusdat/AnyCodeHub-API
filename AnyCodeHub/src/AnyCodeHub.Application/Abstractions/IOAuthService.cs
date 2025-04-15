using Google.Apis.Auth.OAuth2.Responses;

namespace AnyCodeHub.Application.Abstractions;
public interface IOAuthService
{
    public Task<string> GetAuthorizationUrl(string state);
    public Task<string> Callback(string code, string state);
    public Task<TokenResponse> ExchangeCodeForTokenAsync(string code);
}
