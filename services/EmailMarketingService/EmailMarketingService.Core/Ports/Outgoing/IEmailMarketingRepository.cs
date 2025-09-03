using EmailMarketingService.Core.Models;

namespace EmailMarketingService.Core.Ports.Outgoing;

public interface IEmailMarketingRepository
{
    Task<EmailMarketing> CreateAsync(EmailMarketing configuration);
    Task<EmailMarketing?> GetByAccountIdAsync(string accountId);
    Task<EmailMarketing?> GetByIdAsync(string id);
    Task<EmailMarketing?> UpdateAsync(EmailMarketing configuration);
    Task<bool> DeleteByAccountIdAsync(string accountId);
    Task<IQueryable<EmailMarketing>> GetAllAsync();
}