using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.LessonComment;
using static AnyCodeHub.Contract.Services.V1.LessonComment.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class LessonCommentController : ApiController
{
    public LessonCommentController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetLessonComments")]
    [ProducesResponseType(typeof(Result<IEnumerable<LessonCommentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLessonComments(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetLessonCommentQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{lessonCommentId}", Name = "GetLessonCommentById")]
    [ProducesResponseType(typeof(Result<LessonCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLessonComment(Guid lessonCommentId)
    {
        var result = await _sender.Send(new Query.GetLessonCommentByIdQuery(lessonCommentId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateLessonComment")]
    [ProducesResponseType(typeof(Result<LessonCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLessonComment([FromBody] Command.CreateLessonCommentCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{lessonCommentId}", Name = "UpdateLessonComment")]
    [ProducesResponseType(typeof(Result<LessonCommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLessonComment(Guid lessonCommentId, [FromBody] Command.UpdateLessonCommentCommand request)
    {
        var result = await _sender.Send(new Command.UpdateLessonCommentCommand(lessonCommentId, request.lessonId, request.commentId, request.content, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{lessonCommentId}", Name = "DeleteLessonComment")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLessonComment(Guid lessonCommentId, [FromBody] Command.DeleteLessonCommentCommand request)
    {
        var result = await _sender.Send(new Command.DeleteLessonCommentCommand(lessonCommentId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}