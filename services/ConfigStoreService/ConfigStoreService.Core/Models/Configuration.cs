using System.ComponentModel.DataAnnotations;

namespace ConfigStoreService.Core.Models;

public class Configuration
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string AccountId { get; set; } = string.Empty;
    
    [Required]
    public string ServiceType { get; set; } = string.Empty; // EmailMarketing, VideoConferencing
    
    [Required]
    public string ConfigData { get; set; } = string.Empty; // JSON serialized configuration
    
    public int Version { get; set; } = 1;
    
    public bool IsCommitted { get; set; } = false;
    
    [Required]
    public string UserId { get; set; } = string.Empty; // User who created/modified this config
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}