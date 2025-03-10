using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.CourseBenefit;
using static AnyCodeHub.Contract.Services.V1.CourseBenefit.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class CourseBenefitController : ApiController
{
    public CourseBenefitController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetCourseBenefits")]
    [ProducesResponseType(typeof(Result<IEnumerable<CourseBenefitResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseBenefits(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetCourseBenefitQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{courseBenefitId}", Name = "GetCourseBenefitById")]
    [ProducesResponseType(typeof(Result<CourseBenefitResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseBenefit(Guid courseBenefitId)
    {
        var result = await _sender.Send(new Query.GetCourseBenefitByIdQuery(courseBenefitId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateCourseBenefit")]
    [ProducesResponseType(typeof(Result<CourseBenefitResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCourseBenefit([FromBody] Command.CreateCourseBenefitCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{courseBenefitId}", Name = "UpdateCourseBenefit")]
    [ProducesResponseType(typeof(Result<CourseBenefitResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseBenefit(Guid courseBenefitId, [FromBody] Command.UpdateCourseBenefitCommand request)
    {
        var result = await _sender.Send(new Command.UpdateCourseBenefitCommand(courseBenefitId, request.benefitContent));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}