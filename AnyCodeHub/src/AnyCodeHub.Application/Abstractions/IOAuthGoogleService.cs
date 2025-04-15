using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.PeopleService.v1.Data;

namespace AnyCodeHub.Application.Abstractions;
public interface IOAuthGoogleService : IOAuthService
{
    public Task<Person> GetUserInfoAsync(string accessToken, TokenResponse tokenResponse);
}
