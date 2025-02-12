using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AnyCodeHub.Application.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AnyCodeHub.Application.Attributes;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly ICachingService _cachingService;

    public CustomJwtBearerEvents(ICachingService cachingService)
    {
        _cachingService = cachingService;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        //if (context.SecurityToken is Microsoft.IdentityModel.JsonWebTokens.JsonWebToken accessToken)
        //{
        //    var requestToken = accessToken.EncodedToken.ToString();

        //    var email = accessToken.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value;
        //    var authenticated = await _cachingService.GetAsync<Contract.Services.V1.Authentication.Response.AuthenticatedResponse>($"{Contract.Enumerations.CachingPrefixKey.AuthenticatedResponseByUser}_{email}");
        //    if (authenticated is null || authenticated.AccessToken != requestToken)
        //    {
        //        context.Response.Headers.Add("IS-TOKEN-REVOKED", "true");
        //        context.Fail("Invalid access token. Token has been revoked.");
        //    }
        //}
        //else
        //{
        //    context.Fail("Invalid access token.");
        //}
    }
}
