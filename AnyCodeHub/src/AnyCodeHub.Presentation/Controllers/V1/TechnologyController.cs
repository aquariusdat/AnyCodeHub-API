using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.Technology;
using static AnyCodeHub.Contract.Services.V1.Technology.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class TechnologyController : ApiController
{
    public TechnologyController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetTechnologies")]
    [ProducesResponseType(typeof(Result<IEnumerable<TechnologyResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnologies(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetTechnologyQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{technologyId}", Name = "GetTechnologyById")]
    [ProducesResponseType(typeof(Result<TechnologyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTechnology(Guid technologyId)
    {
        var result = await _sender.Send(new Query.GetTechnologyByIdQuery(technologyId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateTechnology")]
    [ProducesResponseType(typeof(Result<TechnologyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTechnology([FromBody] Command.CreateTechnologyCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{technologyId}", Name = "UpdateTechnology")]
    [ProducesResponseType(typeof(Result<TechnologyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTechnology(Guid technologyId, [FromBody] Command.UpdateTechnologyCommand request)
    {
        var result = await _sender.Send(new Command.UpdateTechnologyCommand(technologyId, request.name, request.description));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}