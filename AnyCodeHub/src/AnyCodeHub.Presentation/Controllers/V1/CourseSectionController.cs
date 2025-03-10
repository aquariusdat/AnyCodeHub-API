using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseSection;
using static AnyCodeHub.Contract.Services.V1.CourseSection.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseSectionController : ApiController
{
    public CourseSectionController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseSections")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseSectionResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseSections(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseSectionQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseSectionId}", Name = "GetCourseSectionById")]
    [ProducesResponseType(typeof(Result<CourseSectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseSection(Guid courseSectionId)
    {
        var result = await _sender.Send(new Query.GetCourseSectionByIdQuery(courseSectionId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseSection")]
    [ProducesResponseType(typeof(Result<CourseSectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseSection([FromBody] Command.CreateCourseSectionCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseSectionId}", Name = "UpdateCourseSection")]
    [ProducesResponseType(typeof(Result<CourseSectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseSection(Guid courseSectionId, [FromBody] Command.UpdateCourseSectionCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseSectionCommand(courseSectionId, request.courseId, request.lectureId, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{courseSectionId}", Name = "DeleteCourseSection")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourseSection(Guid courseSectionId, [FromBody] Command.DeleteCourseSectionCommand request)
    {
        var result = await _sender.Send(new Command.DeleteCourseSectionCommand(courseSectionId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}