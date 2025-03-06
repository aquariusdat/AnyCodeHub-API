using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.Services.V1.User;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnyCodeHub.Application.Usecases.V1.Queries.User;
public class ConfirmEmailQueryHandler : IQueryHandler<Query.ConfirmEmailQuery, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ConfirmEmailQueryHandler> _logger;

    public ConfirmEmailQueryHandler(UserManager<AppUser> userManager, ILogger<ConfirmEmailQueryHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(Query.ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserException.UserNotFoundException(request.Email);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (confirmResult is null || !confirmResult.Succeeded) throw new InvalidOperationException($"Email confirmation failed or token expired.");
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while confirming email.");
            throw;
        }
    }
}
