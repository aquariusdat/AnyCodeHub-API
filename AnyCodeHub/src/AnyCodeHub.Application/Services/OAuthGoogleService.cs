
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;
using Newtonsoft.Json;
using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Application.DependencyInjections.Options;

namespace AnyCodeHub.Application.Services;
public class OAuthGoogleService : IOAuthGoogleService
{
    private readonly GoogleApiOptions _googleApiOptions;
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly string[] _scopes = {
        PeopleServiceService.Scope.UserinfoEmail,
        PeopleServiceService.Scope.UserBirthdayRead,
        //PeopleServiceService.Scope.UserGenderRead,
        //PeopleServiceService.Scope.UserAddressesRead,
        PeopleServiceService.Scope.UserPhonenumbersRead,
        PeopleServiceService.Scope.UserinfoProfile,
        //PeopleServiceService.Scope.ContactsReadonly
    };
    private const string applicationName = "AnyCodeHub";

    public OAuthGoogleService(GoogleApiOptions googleApiOptions, IHttpClientFactory httpClientFactory)
    {
        _googleApiOptions = googleApiOptions;
        _httpClientFactory = httpClientFactory;
    }

    public Task<string> Callback(string code, string state)
    {
        throw new NotImplementedException();
    }

    public async Task<TokenResponse> ExchangeCodeForTokenAsync(string code)
    {
        var requestData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("client_id", _googleApiOptions.ClientId),
            new KeyValuePair<string, string>("client_secret", _googleApiOptions.ClientSecret),
            new KeyValuePair<string, string>("redirect_uri", _googleApiOptions.RedirectUri),
            new KeyValuePair<string, string>("grant_type", "authorization_code")
        });

        using (var client = _httpClientFactory.CreateClient())
        {
            var response = await client.PostAsync(_googleApiOptions.TokenUrl, requestData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Token exchange failed with status code {response.StatusCode} and message: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            return tokenResponse;
        }
    }

    public async Task<Person> GetUserInfoAsync(string accessToken, TokenResponse tokenResponse)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleApiOptions.ClientId,
                ClientSecret = _googleApiOptions.ClientSecret
            },
            Scopes = _scopes,
        });
        UserCredential credential = new Google.Apis.Auth.OAuth2.UserCredential(flow, accessToken, tokenResponse);

        BaseClientService.Initializer initializer = new BaseClientService.Initializer()
        {
            HttpClientInitializer = (IConfigurableHttpClientInitializer)credential,
            ApplicationName = applicationName,
            GZipEnabled = true,
        };

        PeopleServiceService service = new PeopleServiceService(initializer);
        var meRequest = service.People.Get("people/me");
        meRequest.PersonFields = _googleApiOptions.PersonFields;
        var me = await meRequest.ExecuteAsync();

        return me;
    }

    public async Task<string> GetAuthorizationUrl(string state)
    {
        var oauth2Client = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleApiOptions.ClientId,
                ClientSecret = _googleApiOptions.ClientSecret,
            },
            Scopes = _scopes,
        });

        // Generate a URL that asks permissions for the Drive activity scope.
        var authorizationUrl = oauth2Client.CreateAuthorizationCodeRequest(_googleApiOptions.RedirectUri);
        authorizationUrl.State = state;

        return authorizationUrl.Build().AbsoluteUri;
    }
}
