using Microsoft.EntityFrameworkCore;
using IdentityService.Core.Models;

namespace IdentityService.Adapters.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
    
    public DbSet<CustomerAccount> CustomerAccounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EmailAddress).IsUnique();
            entity.Property(e => e.Role).HasConversion<string>();
        });
    }
}