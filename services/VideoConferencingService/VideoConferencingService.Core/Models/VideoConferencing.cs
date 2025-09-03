using System.ComponentModel.DataAnnotations;

namespace VideoConferencingService.Core.Models;

public class VideoConferencing
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string AccountId { get; set; } = string.Empty;
    
    [Required]
    public string CalendarIntegration { get; set; } = string.Empty;
    
    [Required]
    public string VideoConferencingProvider { get; set; } = string.Empty;
    
    [Required]
    public string BandwidthOptimization { get; set; } = string.Empty;
    
    public bool ScreenSharing { get; set; } = true;
    
    public bool TranscriptionService { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}