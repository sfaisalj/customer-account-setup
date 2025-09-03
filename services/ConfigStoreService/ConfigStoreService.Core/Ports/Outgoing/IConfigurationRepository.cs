using ConfigStoreService.Core.Models;

namespace ConfigStoreService.Core.Ports.Outgoing;

public interface IConfigurationRepository
{
    Task<Configuration> CreateAsync(Configuration configuration);
    Task<Configuration?> GetByAccountAndServiceAsync(string accountId, string serviceType, bool onlyCommitted = false);
    Task<Configuration?> GetByIdAsync(string id);
    Task<Configuration?> UpdateAsync(Configuration configuration);
    Task<bool> DeleteByAccountAndServiceAsync(string accountId, string serviceType);
    Task<IEnumerable<Configuration>> GetAllByAccountAsync(string accountId);
    Task<IEnumerable<Configuration>> GetHistoryByAccountAndServiceAsync(string accountId, string serviceType);
    Task<IQueryable<Configuration>> GetAllAsync();
}