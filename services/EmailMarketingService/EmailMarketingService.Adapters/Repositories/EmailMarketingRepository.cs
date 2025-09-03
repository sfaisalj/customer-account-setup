using Microsoft.EntityFrameworkCore;
using EmailMarketingService.Core.Models;
using EmailMarketingService.Core.Ports.Outgoing;
using EmailMarketingService.Adapters.Data;

namespace EmailMarketingService.Adapters.Repositories;

public class EmailMarketingRepository : IEmailMarketingRepository
{
    private readonly EmailMarketingDbContext _context;

    public EmailMarketingRepository(EmailMarketingDbContext context)
    {
        _context = context;
    }

    public async Task<EmailMarketing> CreateAsync(EmailMarketing configuration)
    {
        _context.EmailMarketingConfigurations.Add(configuration);
        await _context.SaveChangesAsync();
        return configuration;
    }

    public async Task<EmailMarketing?> GetByAccountIdAsync(string accountId)
    {
        return await _context.EmailMarketingConfigurations
            .FirstOrDefaultAsync(e => e.AccountId == accountId);
    }

    public async Task<EmailMarketing?> GetByIdAsync(string id)
    {
        return await _context.EmailMarketingConfigurations
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EmailMarketing?> UpdateAsync(EmailMarketing configuration)
    {
        _context.EmailMarketingConfigurations.Update(configuration);
        await _context.SaveChangesAsync();
        return configuration;
    }

    public async Task<bool> DeleteByAccountIdAsync(string accountId)
    {
        var configuration = await _context.EmailMarketingConfigurations
            .FirstOrDefaultAsync(e => e.AccountId == accountId);
            
        if (configuration == null)
            return false;

        _context.EmailMarketingConfigurations.Remove(configuration);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IQueryable<EmailMarketing>> GetAllAsync()
    {
        return _context.EmailMarketingConfigurations.AsQueryable();
    }
}