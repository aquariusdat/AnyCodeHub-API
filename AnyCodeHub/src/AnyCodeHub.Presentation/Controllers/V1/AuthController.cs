using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using static AnyCodeHub.Contract.Services.V1.Authentication.Query;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class AuthController : ApiController
{
    public AuthController(ISender sender) : base(sender)
    {
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(typeof(Result<Contract.Services.V1.Authentication.Response.AuthenticatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] Contract.Services.V1.Authentication.Query.Login loginQuery)
    {
        var ip = HttpContext.Connection.RemoteIpAddress;

        var result = await _sender.Send(loginQuery);

        if (result.IsFailure)
            return HandlerFailure(result);

        SetTokenIntoCookies(result);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("Token")]
    [ProducesResponseType(typeof(Result<Contract.Services.V1.Authentication.Response.AuthenticatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Token([FromBody] Contract.Services.V1.Authentication.Query.Token tokenQuery)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        if (!string.IsNullOrEmpty(accessToken))
        {
            tokenQuery.AccessToken = accessToken.ToString();
        }

        var result = await _sender.Send(tokenQuery);

        if (result.IsFailure)
            return HandlerFailure(result);

        SetTokenIntoCookies(result);

        return Ok(result);
    }

    private void SetTokenIntoCookies(Result<Contract.Services.V1.Authentication.Response.AuthenticatedResponse> result)
    {
        Response.Cookies.Append("X-ACCESS-TOKEN", result.Value.AccessToken, new CookieOptions
        {
            Expires = result.Value.AccessTokenExpirationTime,
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
        Response.Cookies.Append("X-REFRESH-TOKEN", result.Value.RefreshToken, new CookieOptions
        {
            Expires = result.Value.RefreshTokenExpirationTime,
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
        //Response.Cookies.Append("X-USER-DATA", JsonConvert.SerializeObject(result.Value.PersonalInformation));
    }

    [Authorize]
    [HttpPost("RevokeToken")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeToken([FromBody] Contract.Services.V1.Authentication.Command.RevokeTokenCommand revokeTokenCommand)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        if (!string.IsNullOrEmpty(accessToken))
        {
            revokeTokenCommand.AccessToken = accessToken.ToString();
        }

        var result = await _sender.Send(revokeTokenCommand);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register([FromBody] Contract.Services.V1.Authentication.Command.RegisterCommand registerCommand)
    {
        var result = await _sender.Send(registerCommand);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("GetUserData")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserData()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        Contract.Services.V1.Authentication.Query.GetUserData getUserDataQuery = new GetUserData(accessToken);

        var result = await _sender.Send(getUserDataQuery);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
