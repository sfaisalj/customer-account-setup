using Microsoft.EntityFrameworkCore;
using IdentityService.Core.Models;
using IdentityService.Core.Ports.Outgoing;
using IdentityService.Adapters.Data;

namespace IdentityService.Adapters.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerAccount?> GetByEmailAsync(string email)
    {
        return await _context.CustomerAccounts
            .FirstOrDefaultAsync(u => u.EmailAddress == email);
    }

    public async Task<CustomerAccount?> GetByIdAsync(string userId)
    {
        return await _context.CustomerAccounts
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<CustomerAccount> CreateAsync(CustomerAccount user)
    {
        _context.CustomerAccounts.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.CustomerAccounts
            .AnyAsync(u => u.EmailAddress == email);
    }

    public async Task<IQueryable<CustomerAccount>> GetAllAsync()
    {
        return _context.CustomerAccounts.AsQueryable();
    }
}