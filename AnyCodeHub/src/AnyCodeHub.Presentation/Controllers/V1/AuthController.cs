using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using static AnyCodeHub.Contract.Services.V1.Authentication.Query;
using static AnyCodeHub.Domain.Exceptions.IdentityException;
using static MassTransit.ValidationResultExtensions;

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
    public async Task<IActionResult> Token()
    {
        //var accessToken = await HttpContext.GetTokenAsync("access_token");
        //if (!string.IsNullOrEmpty(accessToken))
        //{
        //    tokenQuery.AccessToken = accessToken.ToString();
        //}
        try
        {
            var result = await _sender.Send(new Token() { RefreshToken = HttpContext.Request.Cookies["X-REFRESH-TOKEN"].ToString() });

            if (result.IsFailure)
                return HandlerFailure(result);

            SetTokenIntoCookies(result);

            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is TokenException)
            {
                RemoveTokenFromCookies();
            }
            throw;
        }
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
        Response.Cookies.Append("X-USER-DATA", JsonConvert.SerializeObject(result.Value.UserInformation, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }), new CookieOptions
        {
            Expires = result.Value.AccessTokenExpirationTime,
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
    }

    private void RemoveTokenFromCookies()
    {
        Response.Cookies.Append("X-ACCESS-TOKEN", string.Empty, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-99),
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
        Response.Cookies.Append("X-REFRESH-TOKEN", string.Empty, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-99),
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
        Response.Cookies.Append("X-USER-DATA", string.Empty, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-99),
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
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

    [AllowAnonymous]
    [HttpPost("log-out")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Logout()
    {
        RemoveTokenFromCookies();
        return Ok(Contract.Abstractions.Shared.Result.Success());
    }

    [AllowAnonymous]
    [HttpGet("SignInGoogleOAuth")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignInGoogleOAuth()
    {
        string requestOrigin = Request.Headers["Origin"];
        string state = $"{requestOrigin}/";
        Contract.Services.V1.Authentication.Query.SignInGoogleOAuthQuery signIn = new Contract.Services.V1.Authentication.Query.SignInGoogleOAuthQuery(state);

        var authorizationUri = await _sender.Send(signIn);

        if (authorizationUri.IsFailure)
            HandlerFailure(authorizationUri);

        return Ok(authorizationUri);
    }

    [AllowAnonymous]
    [HttpGet("CallbackGoogleOAuth")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CallbackGoogleOAuth([FromQuery] Contract.Services.V1.Authentication.Command.CallbackGoogleOAuthCommand callBack)
    {
        var callbackResponse = await _sender.Send(callBack);

        if (callbackResponse.IsFailure)
            HandlerFailure(callbackResponse);

        SetTokenIntoCookies(callbackResponse.Value);

        return Content($@"
        <html>
        <body>
            <script>
                if (window.opener) {{
                    //// Gửi token và data về window gốc
                    //window.opener.postMessage({{
                    //    type: 'google-auth-success',
                    //}}, window.opener.location.origin);
                
                    // Tự đóng
                    window.close(); // Có thể tự đóng hoặc để window cha đóng
                }}
            </script>
            <p>Authentication successful! This window will close automatically...</p>
        </body>
        </html>", "text/html");

        return Redirect(callBack.state);
    }

}
