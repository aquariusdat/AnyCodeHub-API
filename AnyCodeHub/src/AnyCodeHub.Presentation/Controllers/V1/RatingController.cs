using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Presentation.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnyCodeHub.Contract.Services.V1.Rating;
using static AnyCodeHub.Contract.Services.V1.Rating.Response;
using AnyCodeHub.Contract.Extensions;

namespace AnyCodeHub.Presentation.Controllers.V1;

[ApiVersion(1)]
public class RatingController : ApiController
{
    public RatingController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetRatings")]
    [ProducesResponseType(typeof(Result<IEnumerable<RatingResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRatings(string? searchTerm = null, string? sortColumn = null, string? sortOrder = null, string? sortColumnAndOrder = null, int pageIndex = 1, int pageSize = 10)
    {
        var result = await _sender.Send(new Query.GetRatingQuery(searchTerm, sortColumn, sortOrder.ConvertStringToSortOrder(), sortColumnAndOrder.ConvertStringToSortColumnAndOrder(), pageIndex, pageSize));

        if (result.IsFailure)
            return HandlerFailure(result);
        return Ok(result);
    }

    [HttpGet("{ratingId}", Name = "GetRatingById")]
    [ProducesResponseType(typeof(Result<RatingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRating(Guid ratingId)
    {
        var result = await _sender.Send(new Query.GetRatingByIdQuery(ratingId));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost(Name = "CreateRating")]
    [ProducesResponseType(typeof(Result<RatingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateRating([FromBody] Command.CreateRatingCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{ratingId}", Name = "UpdateRating")]
    [ProducesResponseType(typeof(Result<RatingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRating(Guid ratingId, [FromBody] Command.UpdateRatingCommand request)
    {
        var result = await _sender.Send(new Command.UpdateRatingCommand(ratingId, request.courseId, request.userId, request.rate, request.updatedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{ratingId}", Name = "DeleteRating")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating(Guid ratingId, [FromBody] Command.DeleteRatingCommand request)
    {
        var result = await _sender.Send(new Command.DeleteRatingCommand(ratingId, request.deletedBy));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}