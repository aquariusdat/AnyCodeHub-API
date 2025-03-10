using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.UserCourse;
using static AnyCodeHub.Contract.Services.V1.UserCourse.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class UserCourseController : ApiController
{
    public UserCourseController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetUserCourses")]
    [ProducesResponseType(typeof(Result<IEnumerable<UserCourseResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserCourses(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetUserCourseQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{userCourseId}", Name = "GetUserCourseById")]
    [ProducesResponseType(typeof(Result<UserCourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserCourse(Guid userCourseId)
    {
        var result = await _sender.Send(new Query.GetUserCourseByIdQuery(userCourseId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateUserCourse")]
    [ProducesResponseType(typeof(Result<UserCourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateUserCourse([FromBody] Command.CreateUserCourseCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{userCourseId}", Name = "UpdateUserCourse")]
    [ProducesResponseType(typeof(Result<UserCourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserCourse(Guid userCourseId, [FromBody] Command.UpdateUserCourseCommand request)
    {
        var result = await _sender.Send(new Command.UpdateUserCourseCommand(userCourseId, request.userId, request.courseId, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{userCourseId}", Name = "DeleteUserCourse")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserCourse(Guid userCourseId, [FromBody] Command.DeleteUserCourseCommand request)
    {
        var result = await _sender.Send(new Command.DeleteUserCourseCommand(userCourseId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}