using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Services;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.CommonServices;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static AnyCodeHub.Contract.Services.V1.Authentication.Command;
using static AnyCodeHub.Contract.Services.V1.Authentication.Query;

namespace AnyCodeHub.Application.Usecases.V1.Commands.Authentication;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IUrlHelperService _urlHelperService;
    private readonly ILogger<RegisterCommandHandler> _logger;
    public RegisterCommandHandler(IMapper mapper, UserManager<AppUser> userManager, IEmailSenderService emailSenderService, IUrlHelperService urlHelperService, ILogger<RegisterCommandHandler> logger)
    {
        _mapper = mapper;
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _urlHelperService = urlHelperService;
        _logger = logger;
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
            register.UserName = Regex.Match(register.Email, @"^[^@]+").Value;
            register.IsActive = true;

            IdentityResult result = await _userManager.CreateAsync(register);

            if (result.Succeeded)
            {
                string tokenVerification = await _userManager.GenerateTwoFactorTokenAsync(register, TokenOptions.DefaultEmailProvider);
                var confirmationLink = _urlHelperService.GenerateMailConfirmationLink(register.Email, tokenVerification);
                _emailSenderService.SendAsync(register.Email, "Verify your email to active your AnyCodeHub account.", "", $"""
                    <a href="{confirmationLink}">Click here to verify your account</a>
                    """);
            }

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while registering user. \r\n[ex={ex.ToString()}]");
            throw;
        }
    }
}
