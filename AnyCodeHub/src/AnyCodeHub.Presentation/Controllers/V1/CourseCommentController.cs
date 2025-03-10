using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseComment;
using static AnyCodeHub.Contract.Services.V1.CourseComment.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseCommentController : ApiController
{
    public CourseCommentController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseComments")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseCommentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseComments(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseCommentQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseCommentId}", Name = "GetCourseCommentById")]
    [ProducesResponseType(typeof(Result<CourseCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseComment(Guid courseCommentId)
    {
        var result = await _sender.Send(new Query.GetCourseCommentByIdQuery(courseCommentId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseComment")]
    [ProducesResponseType(typeof(Result<CourseCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseComment([FromBody] Command.CreateCourseCommentCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseCommentId}", Name = "UpdateCourseComment")]
    [ProducesResponseType(typeof(Result<CourseCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseComment(Guid courseCommentId, [FromBody] Command.UpdateCourseCommentCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseCommentCommand(courseCommentId, request.courseId, request.commentId, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{courseCommentId}", Name = "DeleteCourseComment")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourseComment(Guid courseCommentId, [FromBody] Command.DeleteCourseCommentCommand request)
    {
        var result = await _sender.Send(new Command.DeleteCourseCommentCommand(courseCommentId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}