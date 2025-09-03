using Microsoft.EntityFrameworkCore;
using ConfigStoreService.Core.Models;

namespace ConfigStoreService.Adapters.Data;

public class ConfigStoreDbContext : DbContext
{
    public ConfigStoreDbContext(DbContextOptions<ConfigStoreDbContext> options) : base(options) { }
    
    public DbSet<Configuration> Configurations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.AccountId, e.ServiceType });
        });
    }
}