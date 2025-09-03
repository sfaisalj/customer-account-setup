using IdentityService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Adapters.Data;

public static class SeedData
{
    public static async Task Initialize(IdentityDbContext context)
    {
        if (await context.CustomerAccounts.AnyAsync())
            return;

        var customerAccount = new CustomerAccount
        {
            FirstName = "John",
            LastName = "Customer",
            EmailAddress = "customer@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = UserRole.Customer
        };

        var supportAccount = new CustomerAccount
        {
            FirstName = "Jane",
            LastName = "Support",
            EmailAddress = "support@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = UserRole.Support
        };

        context.CustomerAccounts.AddRange(customerAccount, supportAccount);
        await context.SaveChangesAsync();
    }
}