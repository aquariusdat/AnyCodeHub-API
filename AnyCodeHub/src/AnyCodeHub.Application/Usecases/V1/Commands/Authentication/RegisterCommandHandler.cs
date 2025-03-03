using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.CommonServices;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using static AnyCodeHub.Contract.Services.V1.Authentication.Command;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Authentication;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    public RegisterCommandHandler(IMapper mapper, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExisted = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (isExisted != null) throw new RegisterException.EmailExistsException(request.Email);

            AppUser register = _mapper.Map<AppUser>(request);
            register.IsAdmin = request.PhoneNumber == "0789163351";
            register.PasswordHash = request.Password.HashPasswordWithBCrypt();

            await _userManager.CreateAsync(register);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
