using VideoConferencingService.Core.Models;

namespace VideoConferencingService.Core.Ports.Incoming;

public interface IVideoConferencingService
{
    Task<VideoConferencing> CreateConfigurationAsync(string accountId, string calendarIntegration, string videoConferencingProvider, string bandwidthOptimization, bool screenSharing, bool transcriptionService);
    Task<VideoConferencing?> GetConfigurationByAccountIdAsync(string accountId);
    Task<VideoConferencing?> UpdateConfigurationAsync(string accountId, string calendarIntegration, string videoConferencingProvider, string bandwidthOptimization, bool screenSharing, bool transcriptionService);
    Task<bool> DeleteConfigurationAsync(string accountId);
    Task<IEnumerable<VideoConferencing>> GetAllConfigurationsAsync();
    Task<bool> ValidateConfigurationAsync(VideoConferencing configuration);
}

public class VideoConferencingResult
{
    public bool Success { get; set; }
    public VideoConferencing? Configuration { get; set; }
    public string? Error { get; set; }
}