using EmailMarketingService.Core.Models;

namespace EmailMarketingService.Core.Ports.Incoming;

public interface IEmailMarketingService
{
    Task<EmailMarketing> CreateConfigurationAsync(string accountId, string emailDeliveryInfra, string emailTemplate, string analytics, string abTesting);
    Task<EmailMarketing?> GetConfigurationByAccountIdAsync(string accountId);
    Task<EmailMarketing?> UpdateConfigurationAsync(string accountId, string emailDeliveryInfra, string emailTemplate, string analytics, string abTesting);
    Task<bool> DeleteConfigurationAsync(string accountId);
    Task<IEnumerable<EmailMarketing>> GetAllConfigurationsAsync();
    Task<bool> ValidateConfigurationAsync(EmailMarketing configuration);
}

public class EmailMarketingResult
{
    public bool Success { get; set; }
    public EmailMarketing? Configuration { get; set; }
    public string? Error { get; set; }
}