using IdentityService.Core.Models;

namespace IdentityService.Core.Ports.Outgoing;

public interface IUserRepository
{
    Task<CustomerAccount?> GetByEmailAsync(string email);
    Task<CustomerAccount?> GetByIdAsync(string userId);
    Task<CustomerAccount> CreateAsync(CustomerAccount user);
    Task<bool> EmailExistsAsync(string email);
    Task<IQueryable<CustomerAccount>> GetAllAsync();
}