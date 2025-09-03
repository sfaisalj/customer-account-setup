using Microsoft.EntityFrameworkCore;
using EmailMarketingService.Core.Models;

namespace EmailMarketingService.Adapters.Data;

public class EmailMarketingDbContext : DbContext
{
    public EmailMarketingDbContext(DbContextOptions<EmailMarketingDbContext> options) : base(options) { }
    
    public DbSet<EmailMarketing> EmailMarketingConfigurations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailMarketing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AccountId).IsUnique();
        });
    }
}