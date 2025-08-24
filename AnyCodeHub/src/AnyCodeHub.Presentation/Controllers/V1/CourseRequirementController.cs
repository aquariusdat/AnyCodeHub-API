using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseRequirement;
using static AnyCodeHub.Contract.Services.V1.CourseRequirement.Response;
using AnyCodeHub.Contract.Extensions;
using System.Security.Claims;

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

    [HttpDelete("{courseRequirementId}", Name = "DeleteCourseRequirement")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourseRequirement(Guid courseRequirementId)
    {
        // Several options to get the current user ID:

        // Option 1: Get user ID from claims if available
        // var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Guid currentUserId = userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty;
        string a = "123";

        // Option 2: For testing purposes, use a fixed value
        Guid currentUserId = Guid.Empty; // Replace with actual user ID in production

        // Option 3: If using identity framework, could use:
        // Guid currentUserId = Guid.Parse(User.Identity.GetUserId());

        var result = await _sender.Send(new Command.DeleteCourseRequirementCommand(courseRequirementId, currentUserId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }




}