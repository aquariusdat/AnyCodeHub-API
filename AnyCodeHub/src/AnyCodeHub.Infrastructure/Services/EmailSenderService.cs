using AnyCodeHub.Contract.Abstractions.Services;
using AnyCodeHub.Infrastructure.DependencyInjections.Options;
using Microsoft.Extensions.Logging;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace AnyCodeHub.Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly ILogger<EmailSenderService> _logger;
    private readonly MailSettings _mailSettings;
    public EmailSenderService(ILogger<EmailSenderService> logger, MailSettings mailSettings)
    {
        _logger = logger;
        _mailSettings = mailSettings;
    }
    public async Task<bool> SendAsync(string EmailTo, string Title, string TextContext, string HTMLContent)
    {
        MimeMessage email_Message = new MimeMessage();
        email_Message.From.Add(MailboxAddress.Parse(_mailSettings.Email));
        email_Message.To.Add(MailboxAddress.Parse(EmailTo));

        email_Message.Subject = Title;
        BodyBuilder emailBodyBuilder = new BodyBuilder();

        emailBodyBuilder.TextBody = TextContext;
        emailBodyBuilder.HtmlBody = HTMLContent;

        email_Message.Body = emailBodyBuilder.ToMessageBody();
        //email_Message.Body = new TextPart(TextFormat.Html) { Text = Content };

        //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
        using (SmtpClient MailClient = new SmtpClient())
        {
            MailClient.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
            MailClient.Authenticate(_mailSettings.Email, _mailSettings.Password);
            MailClient.Send(email_Message);
            MailClient.Disconnect(true);
            MailClient.Dispose();
        }
            
        return true;
    }
}
