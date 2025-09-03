using Microsoft.EntityFrameworkCore;
using VideoConferencingService.Core.Models;

namespace VideoConferencingService.Adapters.Data;

public class VideoConferencingDbContext : DbContext
{
    public VideoConferencingDbContext(DbContextOptions<VideoConferencingDbContext> options) : base(options) { }
    
    public DbSet<VideoConferencing> VideoConferencingConfigurations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoConferencing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AccountId).IsUnique();
        });
    }
}