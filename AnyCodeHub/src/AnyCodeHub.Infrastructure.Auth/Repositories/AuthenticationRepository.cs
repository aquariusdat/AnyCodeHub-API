using AnyCodeHub.Domain.Abstractions;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.ValueObjects;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using AnyCodeHub.Infrastructure.Auth.Abstractions.Auth;
using Microsoft.AspNetCore.Identity;

namespace AnyCodeHub.Infrastructure.Auth.Repositories;
public class AuthenticationRepository : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IRepositoryBase<Domain.Entities.Identity.AppUser, Guid> _repositoryBase;
    private readonly ITokenGeneratorService _tokenGenerator;
    private readonly UserManager<AppUser> _userManager;
    //private readonly ICachingService
    public AuthenticationRepository(IUnitOfWork unitOfWork, UserManager<AppUser> userManager /*, IRepositoryBase<Domain.Entities.Identity.AppUser, Guid> repositoryBase*/, ITokenGeneratorService tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        //_repositoryBase = repositoryBase;
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
    }

    public Task<AuthToken> GetNewTokenByRefreshToken(AuthToken AuthToken)
    {
        throw new Exception();
    }
}
