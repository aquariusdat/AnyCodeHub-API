namespace AnyCodeHub.Contract.Abstractions.Services;

public interface IEmailSenderService
{
    Task<bool> SendAsync(string EmailTo, string Title, string TextContent, string HTMLContent);
}
