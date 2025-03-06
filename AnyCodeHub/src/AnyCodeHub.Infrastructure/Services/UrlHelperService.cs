using AnyCodeHub.Contract.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace AnyCodeHub.Infrastructure.Services;

public class UrlHelperService : IUrlHelperService
{
    private readonly IHttpContextAccessor _contextAccessor;
    public UrlHelperService(IHttpContextAccessor contextAccessor )
    {
        _contextAccessor = contextAccessor;
    }
    public string GenerateMailConfirmationLink(string Email, string Token)
    {
        var request = _contextAccessor.HttpContext?.Request;
        if (request is null) throw new InvalidOperationException($"Cannot generate Url outside of an Http context.");

        var scheme = request.Scheme;
        var host = request.Host;
        var pathBase = request.PathBase;

        return $"{scheme}://{host}{pathBase}/user/confirm-email?Email={Email}&Token={HttpUtility.UrlEncode(Token)}";
    }
}
