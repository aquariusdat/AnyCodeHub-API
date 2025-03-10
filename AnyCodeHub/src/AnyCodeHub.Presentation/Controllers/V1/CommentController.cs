using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.Comment;
using static AnyCodeHub.Contract.Services.V1.Comment.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CommentController : ApiController
{
    public CommentController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetComments")]
    [ProducesResponseType(typeof(Result<IEnumerable<CommentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComments(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCommentQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{commentId}", Name = "GetCommentById")]
    [ProducesResponseType(typeof(Result<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComment(Guid commentId)
    {
        var result = await _sender.Send(new Query.GetCommentByIdQuery(commentId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateComment")]
    [ProducesResponseType(typeof(Result<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateComment([FromBody] Command.CreateCommentCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{commentId}", Name = "UpdateComment")]
    [ProducesResponseType(typeof(Result<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] Command.UpdateCommentCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCommentCommand(commentId, request.content, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{commentId}", Name = "DeleteComment")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteComment(Guid commentId, [FromBody] Command.DeleteCommentCommand request)
    {
        var result = await _sender.Send(new Command.DeleteCommentCommand(commentId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}