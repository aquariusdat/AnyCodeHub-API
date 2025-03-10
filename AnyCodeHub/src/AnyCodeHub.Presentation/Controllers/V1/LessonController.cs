using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.Lesson;
using static AnyCodeHub.Contract.Services.V1.Lesson.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class LessonController : ApiController
{
    public LessonController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetLessons")]
    [ProducesResponseType(typeof(Result<IEnumerable<LessonResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLessons(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetLessonQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{lessonId}", Name = "GetLessonById")]
    [ProducesResponseType(typeof(Result<LessonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var result = await _sender.Send(new Query.GetLessonByIdQuery(lessonId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateLesson")]
    [ProducesResponseType(typeof(Result<LessonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLesson([FromBody] Command.CreateLessonCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{lessonId}", Name = "UpdateLesson")]
    [ProducesResponseType(typeof(Result<LessonResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLesson(Guid lessonId, [FromBody] Command.UpdateLessonCommand request)
    {
        var result = await _sender.Send(new Command.UpdateLessonCommand(lessonId, request.title, request.description, request.videoUrl, request.pdfUrl, request.sectionId, request.courseId, request.duration, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{lessonId}", Name = "DeleteLesson")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLesson(Guid lessonId, [FromBody] Command.DeleteLessonCommand request)
    {
        var result = await _sender.Send(new Command.DeleteLessonCommand(lessonId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}