using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.SectionLesson;
using static AnyCodeHub.Contract.Services.V1.SectionLesson.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class SectionLessonController : ApiController
{
    public SectionLessonController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetSectionLessons")]
    [ProducesResponseType(typeof(Result<IEnumerable<SectionLessonResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSectionLessons(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetSectionLessonQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{sectionLessonId}", Name = "GetSectionLessonById")]
    [ProducesResponseType(typeof(Result<SectionLessonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSectionLesson(Guid sectionLessonId)
    {
        var result = await _sender.Send(new Query.GetSectionLessonByIdQuery(sectionLessonId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateSectionLesson")]
    [ProducesResponseType(typeof(Result<SectionLessonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSectionLesson([FromBody] Command.CreateSectionLessonCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{sectionLessonId}", Name = "UpdateSectionLesson")]
    [ProducesResponseType(typeof(Result<SectionLessonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSectionLesson(Guid sectionLessonId, [FromBody] Command.UpdateSectionLessonCommand request)
    {
        var result = await _sender.Send(new Command.UpdateSectionLessonCommand(sectionLessonId, request.sectionId, request.lessonId, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{sectionLessonId}", Name = "DeleteSectionLesson")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSectionLesson(Guid sectionLessonId, [FromBody] Command.DeleteSectionLessonCommand request)
    {
        var result = await _sender.Send(new Command.DeleteSectionLessonCommand(sectionLessonId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}