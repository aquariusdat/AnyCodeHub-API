namespace AnyCodeHub.Infrastructure.Auth.DependencyInjections.Options;
public class JwtOptions
{
    public string SecretKey { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public long ExpirationMinutes { get; set; }
    public long RefreshTokenValidityInDays { get; set; }
    public string PrivateKeyPath { get; set; }
}
