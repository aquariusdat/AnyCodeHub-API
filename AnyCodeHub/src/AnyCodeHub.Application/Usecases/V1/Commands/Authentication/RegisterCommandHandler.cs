using AnyCodeHub.Contract.Abstractions.Message;
using AnyCodeHub.Contract.Abstractions.Services;
using AnyCodeHub.Contract.Abstractions.Shared;
using AnyCodeHub.Contract.CommonServices;
using AnyCodeHub.Contract.Enumerations;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
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
    private readonly IHttpContextAccessor _contextAccessor;
    public RegisterCommandHandler(IMapper mapper, UserManager<AppUser> userManager, IEmailSenderService emailSenderService, IUrlHelperService urlHelperService, ILogger<RegisterCommandHandler> logger, IHttpContextAccessor contextAccessor = null)
    {
        _mapper = mapper;
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _urlHelperService = urlHelperService;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }
    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExisted = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (isExisted != null) throw new RegisterException.EmailExistsException(request.Email);

            AppUser register = _mapper.Map<AppUser>(request);
            register.IsAdmin = request.Roles?.Any(t => t.ToUpper() == UserRole.ADMIN) ?? false;
            register.PasswordHash = request.Password.HashPasswordWithBCrypt();
            register.UserName = Regex.Match(register.Email, @"^[^@]+").Value;

            IdentityResult result = await _userManager.CreateAsync(register);

            if (result.Succeeded)
            {
                if(request.Roles?.Count() > 0)
                {
                    await _userManager.AddToRolesAsync(register, request.Roles);
                }
                else
                {
                    await _userManager.AddToRoleAsync(register, UserRole.USER);
                }

                var clientUrl = _contextAccessor.HttpContext.Request.Headers["Referer"].ToString();
                if (string.IsNullOrEmpty(clientUrl))
                {
                    var scheme = _contextAccessor.HttpContext.Request.Scheme;
                    var host = _contextAccessor.HttpContext.Request.Host;
                    clientUrl = $"{scheme}://{host}";
                }
                string tokenVerification = await _userManager.GenerateEmailConfirmationTokenAsync(register);
                var confirmationLink = _urlHelperService.GenerateMailConfirmationLink(register.Email, tokenVerification, clientUrl);
                _emailSenderService.SendAsync(register.Email, "Verify your email to active your AnyCodeHub account.", "", $"""
                    <div style="">
                        <div><h3>To verify your email  address <a href="mailto:{register.Email.ToLower()}" target="_blank">{register.Email.ToLower()}</a> visit the following link:</h3></div>
                        <div><a href="{confirmationLink}">Click here to verify your account</a></div>
                        <p style=">If you did not request this verification, please ignore this email. If you feel something is wrong, please contact us: web_feedback@anycodehub.com.</p>
                        <p style="margin-bottom:0;">Please note that this link is only valid for the next 2 hours only. </p>
                        <p style="margin-top:0;">* Don't forward this email or verification code to anyone.</p>
                        <p style="margin-bottom:0;">Note - This email is sent automatically and you do not need to reply.</p>
                        <p style="margin:0;">The AnyCodeHub Team</p>
                        <p style="margin:0;">www.anycodehub.com</p>
                    </div>
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
