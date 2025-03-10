using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseTechnology;
using static AnyCodeHub.Contract.Services.V1.CourseTechnology.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseTechnologyController : ApiController
{
    public CourseTechnologyController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseTechnologies")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseTechnologyResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseTechnologies(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseTechnologyQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseTechnologyId}", Name = "GetCourseTechnologyById")]
    [ProducesResponseType(typeof(Result<CourseTechnologyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseTechnology(Guid courseTechnologyId)
    {
        var result = await _sender.Send(new Query.GetCourseTechnologyByIdQuery(courseTechnologyId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseTechnology")]
    [ProducesResponseType(typeof(Result<CourseTechnologyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseTechnology([FromBody] Command.CreateCourseTechnologyCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseTechnologyId}", Name = "UpdateCourseTechnology")]
    [ProducesResponseType(typeof(Result<CourseTechnologyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseTechnology(Guid courseTechnologyId, [FromBody] Command.UpdateCourseTechnologyCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseTechnologyCommand(courseTechnologyId, request.courseId, request.technologyId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}