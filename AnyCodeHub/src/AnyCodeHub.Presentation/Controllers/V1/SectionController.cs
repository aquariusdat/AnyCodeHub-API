using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.Section;
using static AnyCodeHub.Contract.Services.V1.Section.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class SectionController : ApiController
{
    public SectionController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetSections")]
    [ProducesResponseType(typeof(Result<IEnumerable<SectionResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSections(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetSectionQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{sectionId}", Name = "GetSectionById")]
    [ProducesResponseType(typeof(Result<SectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSection(Guid sectionId)
    {
        var result = await _sender.Send(new Query.GetSectionByIdQuery(sectionId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateSection")]
    [ProducesResponseType(typeof(Result<SectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSection([FromBody] Command.CreateSectionCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{sectionId}", Name = "UpdateSection")]
    [ProducesResponseType(typeof(Result<SectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSection(Guid sectionId, [FromBody] Command.UpdateSectionCommand request)
    {
        var result = await _sender.Send(new Command.UpdateSectionCommand(sectionId, request.name, request.courseId, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{sectionId}", Name = "DeleteSection")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSection(Guid sectionId, [FromBody] Command.DeleteSectionCommand request)
    {
        var result = await _sender.Send(new Command.DeleteSectionCommand(sectionId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}