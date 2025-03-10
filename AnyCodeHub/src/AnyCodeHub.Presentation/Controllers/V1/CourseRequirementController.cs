using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseRequirement;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseRequirementController : ApiController
{
    public CourseRequirementController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseRequirements")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseRequirementResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseRequirements(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseRequirementQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseRequirementId}", Name = "GetCourseRequirementById")]
    [ProducesResponseType(typeof(Result<CourseRequirementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseRequirement(Guid courseRequirementId)
    {
        var result = await _sender.Send(new Query.GetCourseRequirementByIdQuery(courseRequirementId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseRequirement")]
    [ProducesResponseType(typeof(Result<CourseRequirementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseRequirement([FromBody] Command.CreateCourseRequirementCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseRequirementId}", Name = "UpdateCourseRequirement")]
    [ProducesResponseType(typeof(Result<CourseRequirementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseRequirement(Guid courseRequirementId, [FromBody] Command.UpdateCourseRequirementCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseRequirementCommand(courseRequirementId, request.requirementContent));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}