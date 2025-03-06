using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.User;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnyCodeHub.Presentation.Controllers.V1
{
    [ApiVersion(1)]
    public class UserController : ApiController
    {
        public UserController(ISender sender) : base(sender)
        {
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        [ProducesResponseType(typeof(Result<Contract.Services.V1.Authentication.Response.AuthenticatedResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> User(string Email, string Token)
        {
            var result = await _sender.Send(new Query.ConfirmEmailQuery(Email, Token));

            if (result.IsFailure)
                return HandlerFailure(result);

            return Ok(result);
        }
    }
}
