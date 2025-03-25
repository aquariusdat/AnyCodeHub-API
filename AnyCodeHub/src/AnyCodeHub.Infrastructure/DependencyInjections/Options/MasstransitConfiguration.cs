namespace AnyCodeHub.Infrastructure.DependencyInjections.Options;
public record MasstransitConfiguration
{
    public string Host { get; set; }
    public string VHost { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string EmailQueueName { get; set; }
    public string SmsQueueName { get; set; }
    public string PostCreatedQueueName { get; set; }
    public string DataCreatedExchangeName { get; set; }
    public string OutboxMessageQuueName { get; set; }
    public string SendMailWhenUserRegistered { get; set; }
}
