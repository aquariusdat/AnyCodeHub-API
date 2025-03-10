using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseCategory;
using static AnyCodeHub.Contract.Services.V1.CourseCategory.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseCategoryController : ApiController
{
    public CourseCategoryController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseCategories")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseCategoryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseCategories(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseCategoryQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseCategoryId}", Name = "GetCourseCategoryById")]
    [ProducesResponseType(typeof(Result<CourseCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseCategory(Guid courseCategoryId)
    {
        var result = await _sender.Send(new Query.GetCourseCategoryByIdQuery(courseCategoryId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseCategory")]
    [ProducesResponseType(typeof(Result<CourseCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseCategory([FromBody] Command.CreateCourseCategoryCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseCategoryId}", Name = "UpdateCourseCategory")]
    [ProducesResponseType(typeof(Result<CourseCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseCategory(Guid courseCategoryId, [FromBody] Command.UpdateCourseCategoryCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseCategoryCommand(courseCategoryId, request.courseId, request.categoryId, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{courseCategoryId}", Name = "DeleteCourseCategory")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourseCategory(Guid courseCategoryId, [FromBody] Command.DeleteCourseCategoryCommand request)
    {
        var result = await _sender.Send(new Command.DeleteCourseCategoryCommand(courseCategoryId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}