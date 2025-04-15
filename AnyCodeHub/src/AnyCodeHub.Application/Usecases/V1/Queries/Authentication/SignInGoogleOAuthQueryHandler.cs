using AnyCodeHub.Application.Abstractions;
using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using static AnyCodeHub.Contract.Services.V1.Authentication.Query;

namespace AnyCodeHub.Application.Usecases.V1.Queries.Authentication;
public class SignInGoogleOAuthQueryHandler : IQueryHandler<SignInGoogleOAuthQuery, string>
{
    private readonly IOAuthGoogleService _oAuthGoogleService;

    public SignInGoogleOAuthQueryHandler(IOAuthGoogleService oAuthGoogleService)
    {
        _oAuthGoogleService = oAuthGoogleService;
    }

    public async Task<Result<string>> Handle(SignInGoogleOAuthQuery request, CancellationToken cancellationToken)
    {
        return await _oAuthGoogleService.GetAuthorizationUrl(request.state);
    }
}
