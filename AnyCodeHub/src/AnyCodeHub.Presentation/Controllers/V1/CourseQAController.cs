using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseQA;
using static AnyCodeHub.Contract.Services.V1.CourseQA.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseQAController : ApiController
{
    public CourseQAController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseQAs")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseQAResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseQAs(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseQAQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseQAId}", Name = "GetCourseQAById")]
    [ProducesResponseType(typeof(Result<CourseQAResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseQA(Guid courseQAId)
    {
        var result = await _sender.Send(new Query.GetCourseQAByIdQuery(courseQAId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseQA")]
    [ProducesResponseType(typeof(Result<CourseQAResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseQA([FromBody] Command.CreateCourseQACommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseQAId}", Name = "UpdateCourseQA")]
    [ProducesResponseType(typeof(Result<CourseQAResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseQA(Guid courseQAId, [FromBody] Command.UpdateCourseQACommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseQACommand(courseQAId, request.question, request.answer, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}