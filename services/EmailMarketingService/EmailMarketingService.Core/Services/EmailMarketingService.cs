using EmailMarketingService.Core.Models;
using EmailMarketingService.Core.Ports.Incoming;
using EmailMarketingService.Core.Ports.Outgoing;

namespace EmailMarketingService.Core.Services;

public class EmailMarketingService : IEmailMarketingService
{
    private readonly IEmailMarketingRepository _repository;

    public EmailMarketingService(IEmailMarketingRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailMarketing> CreateConfigurationAsync(string accountId, string emailDeliveryInfra, string emailTemplate, string analytics, string abTesting)
    {
        var existingConfig = await _repository.GetByAccountIdAsync(accountId);
        if (existingConfig != null)
        {
            throw new InvalidOperationException($"Configuration already exists for account {accountId}");
        }

        if (!EmailDeliveryProvider.SupportedProviders.Contains(emailDeliveryInfra))
        {
            throw new ArgumentException($"Unsupported email delivery provider: {emailDeliveryInfra}");
        }

        var configuration = new EmailMarketing
        {
            AccountId = accountId,
            EmailDeliveryInfra = emailDeliveryInfra,
            EmailTemplate = emailTemplate,
            Analytics = analytics,
            ABTesting = abTesting
        };

        return await _repository.CreateAsync(configuration);
    }

    public async Task<EmailMarketing?> GetConfigurationByAccountIdAsync(string accountId)
    {
        return await _repository.GetByAccountIdAsync(accountId);
    }

    public async Task<EmailMarketing?> UpdateConfigurationAsync(string accountId, string emailDeliveryInfra, string emailTemplate, string analytics, string abTesting)
    {
        var existingConfig = await _repository.GetByAccountIdAsync(accountId);
        if (existingConfig == null)
        {
            return null;
        }

        if (!EmailDeliveryProvider.SupportedProviders.Contains(emailDeliveryInfra))
        {
            throw new ArgumentException($"Unsupported email delivery provider: {emailDeliveryInfra}");
        }

        existingConfig.EmailDeliveryInfra = emailDeliveryInfra;
        existingConfig.EmailTemplate = emailTemplate;
        existingConfig.Analytics = analytics;
        existingConfig.ABTesting = abTesting;
        existingConfig.UpdatedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(existingConfig);
    }

    public async Task<bool> DeleteConfigurationAsync(string accountId)
    {
        return await _repository.DeleteByAccountIdAsync(accountId);
    }

    public async Task<IEnumerable<EmailMarketing>> GetAllConfigurationsAsync()
    {
        var queryable = await _repository.GetAllAsync();
        return queryable.ToList();
    }

    public async Task<bool> ValidateConfigurationAsync(EmailMarketing configuration)
    {
        if (string.IsNullOrEmpty(configuration.AccountId) ||
            string.IsNullOrEmpty(configuration.EmailDeliveryInfra) ||
            string.IsNullOrEmpty(configuration.EmailTemplate) ||
            string.IsNullOrEmpty(configuration.Analytics) ||
            string.IsNullOrEmpty(configuration.ABTesting))
        {
            return false;
        }

        return EmailDeliveryProvider.SupportedProviders.Contains(configuration.EmailDeliveryInfra);
    }
}