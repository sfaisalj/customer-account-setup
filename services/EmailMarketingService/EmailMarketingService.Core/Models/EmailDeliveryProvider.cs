namespace EmailMarketingService.Core.Models;

public static class EmailDeliveryProvider
{
    public const string SES = "SES";
    public const string Mailchimp = "Mailchimp";
    public const string SendGrid = "SendGrid";
    
    public static readonly string[] SupportedProviders = [SES, Mailchimp, SendGrid];
}