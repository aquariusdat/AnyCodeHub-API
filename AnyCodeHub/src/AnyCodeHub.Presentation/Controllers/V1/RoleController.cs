using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Extensions;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AnyCodeHub.Contract.Services.V1.Rating.Response;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using AnyCodeHub.Contract.Enumerations;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
[Authorize(Roles = "ADMIN,MODERATOR")]
public class RoleController : ApiController
{
    public RoleController(ISender sender) : base(sender)
    {
    }


    [HttpPost("AddRole")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRole([FromBody] Contract.Services.V1.Role.Command.AddRoleCommand addRoleCommand)
    {
        var result = await _sender.Send(addRoleCommand);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("AddAppUserRole")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddAppUserRole([FromBody] Contract.Services.V1.Role.Command.AddAppUserRoleCommand addAppUserRoleCommand)
    {
        var result = await _sender.Send(addAppUserRoleCommand);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("GetAppUserRole")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppUserRole(string Email)
    {
        var result = await _sender.Send(new Contract.Services.V1.Role.Query.GetAppUserRoleQuery(Email));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("GetRoleQuery")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleQuery()
    {
        var result = await _sender.Send(new Contract.Services.V1.Role.Query.GetRoleQuery());

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
