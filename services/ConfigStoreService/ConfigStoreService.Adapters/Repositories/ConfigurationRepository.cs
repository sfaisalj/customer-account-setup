using Microsoft.EntityFrameworkCore;
using ConfigStoreService.Core.Models;
using ConfigStoreService.Core.Ports.Outgoing;
using ConfigStoreService.Adapters.Data;

namespace ConfigStoreService.Adapters.Repositories;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly ConfigStoreDbContext _context;

    public ConfigurationRepository(ConfigStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Configuration> CreateAsync(Configuration configuration)
    {
        _context.Configurations.Add(configuration);
        await _context.SaveChangesAsync();
        return configuration;
    }

    public async Task<Configuration?> GetByAccountAndServiceAsync(string accountId, string serviceType, bool onlyCommitted = false)
    {
        var query = _context.Configurations
            .Where(c => c.AccountId == accountId && c.ServiceType == serviceType);

        if (onlyCommitted)
        {
            query = query.Where(c => c.IsCommitted);
        }

        return await query
            .OrderByDescending(c => c.UpdatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<Configuration?> GetByIdAsync(string id)
    {
        return await _context.Configurations
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Configuration?> UpdateAsync(Configuration configuration)
    {
        _context.Configurations.Update(configuration);
        await _context.SaveChangesAsync();
        return configuration;
    }

    public async Task<bool> DeleteByAccountAndServiceAsync(string accountId, string serviceType)
    {
        var configurations = await _context.Configurations
            .Where(c => c.AccountId == accountId && c.ServiceType == serviceType)
            .ToListAsync();

        if (!configurations.Any())
            return false;

        _context.Configurations.RemoveRange(configurations);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Configuration>> GetAllByAccountAsync(string accountId)
    {
        return await _context.Configurations
            .Where(c => c.AccountId == accountId)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Configuration>> GetHistoryByAccountAndServiceAsync(string accountId, string serviceType)
    {
        return await _context.Configurations
            .Where(c => c.AccountId == accountId && c.ServiceType == serviceType)
            .OrderByDescending(c => c.Version)
            .ToListAsync();
    }

    public async Task<IQueryable<Configuration>> GetAllAsync()
    {
        return _context.Configurations.AsQueryable();
    }
}