namespace AnyCodeHub.Infrastructure.DependencyInjections.Options;

public record MailSettings
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
}
