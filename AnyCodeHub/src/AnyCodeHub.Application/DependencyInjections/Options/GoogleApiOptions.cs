namespace AnyCodeHub.Application.DependencyInjections.Options;
public class GoogleApiOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
    public string PersonFields { get; set; }
    public string TokenUrl { get; set; }
}
