namespace AnyCodeHub.Domain.ValueObjects;
public class AuthToken
{
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiryTime { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
