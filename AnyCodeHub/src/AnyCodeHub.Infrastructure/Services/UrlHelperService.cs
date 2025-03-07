using AnyCodeHub.Contract.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AnyCodeHub.Infrastructure.Services;

public class UrlHelperService : IUrlHelperService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<UrlHelperService> _logger;
    public UrlHelperService(IHttpContextAccessor contextAccessor, ILogger<UrlHelperService> logger)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
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

    public string GenerateSlug(string Name)
    {
        try
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name)) return string.Empty;

            string normalizedString = Name.ToLower().Normalize(NormalizationForm.FormD);
            string withoutAccents = new string(normalizedString
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
            string slug = Regex.Replace(withoutAccents, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9-]", "");
            slug = slug.Trim('-');
            return slug;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while generating slug. [Error={ex.ToString()}]");
            throw;
        }
    }
}
