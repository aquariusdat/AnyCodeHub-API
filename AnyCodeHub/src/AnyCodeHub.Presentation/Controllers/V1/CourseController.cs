using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.Course;
using static AnyCodeHub.Contract.Services.V1.Course.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseController : ApiController
{
    protected CourseController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourses")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Courses(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseId}")]
    [ProducesResponseType(typeof(Result<CourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Courses(Guid courseId)
    {
        var result = await _sender.Send(new Query.GetPostByIdQuery(courseId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourse")]
    [ProducesResponseType(typeof(Result<CourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Courses([FromBody] Command.CreateCourseCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut(Name = "UpdateCourse")]
    [ProducesResponseType(typeof(Result<CourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Courses(Guid courseId, [FromBody] Command.UpdateCourseCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseCommand(courseId, request.name, request.description, request.price, request.salePrice, request.imageUrl, request.videoUrl, request.slug, request.status, request.authorId, request.level, request.totalViews, request.totalDuration, request.rating, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete(Name = "DeletePost")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Courses(Guid courseId, [FromBody] Command.DeleteCourseCommand request)
    {
        var result = await _sender.Send(new Command.DeleteCourseCommand(courseId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
