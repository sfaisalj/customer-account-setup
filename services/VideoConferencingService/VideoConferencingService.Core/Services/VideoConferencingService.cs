using VideoConferencingService.Core.Models;
using VideoConferencingService.Core.Ports.Incoming;
using VideoConferencingService.Core.Ports.Outgoing;

namespace VideoConferencingService.Core.Services;

public class VideoConferencingService : IVideoConferencingService
{
    private readonly IVideoConferencingRepository _repository;

    public VideoConferencingService(IVideoConferencingRepository repository)
    {
        _repository = repository;
    }

    public async Task<VideoConferencing> CreateConfigurationAsync(string accountId, string calendarIntegration, string videoConferencingProvider, string bandwidthOptimization, bool screenSharing, bool transcriptionService)
    {
        var existingConfig = await _repository.GetByAccountIdAsync(accountId);
        if (existingConfig != null)
        {
            throw new InvalidOperationException($"Configuration already exists for account {accountId}");
        }

        if (!VideoConferencingProvider.SupportedProviders.Contains(videoConferencingProvider))
        {
            throw new ArgumentException($"Unsupported video conferencing provider: {videoConferencingProvider}");
        }

        if (!CalendarIntegration.SupportedIntegrations.Contains(calendarIntegration))
        {
            throw new ArgumentException($"Unsupported calendar integration: {calendarIntegration}");
        }

        if (!BandwidthOptimization.SupportedOptimizations.Contains(bandwidthOptimization))
        {
            throw new ArgumentException($"Unsupported bandwidth optimization: {bandwidthOptimization}");
        }

        var configuration = new VideoConferencing
        {
            AccountId = accountId,
            CalendarIntegration = calendarIntegration,
            VideoConferencingProvider = videoConferencingProvider,
            BandwidthOptimization = bandwidthOptimization,
            ScreenSharing = screenSharing,
            TranscriptionService = transcriptionService
        };

        return await _repository.CreateAsync(configuration);
    }

    public async Task<VideoConferencing?> GetConfigurationByAccountIdAsync(string accountId)
    {
        return await _repository.GetByAccountIdAsync(accountId);
    }

    public async Task<VideoConferencing?> UpdateConfigurationAsync(string accountId, string calendarIntegration, string videoConferencingProvider, string bandwidthOptimization, bool screenSharing, bool transcriptionService)
    {
        var existingConfig = await _repository.GetByAccountIdAsync(accountId);
        if (existingConfig == null)
        {
            return null;
        }

        if (!VideoConferencingProvider.SupportedProviders.Contains(videoConferencingProvider))
        {
            throw new ArgumentException($"Unsupported video conferencing provider: {videoConferencingProvider}");
        }

        if (!CalendarIntegration.SupportedIntegrations.Contains(calendarIntegration))
        {
            throw new ArgumentException($"Unsupported calendar integration: {calendarIntegration}");
        }

        if (!BandwidthOptimization.SupportedOptimizations.Contains(bandwidthOptimization))
        {
            throw new ArgumentException($"Unsupported bandwidth optimization: {bandwidthOptimization}");
        }

        existingConfig.CalendarIntegration = calendarIntegration;
        existingConfig.VideoConferencingProvider = videoConferencingProvider;
        existingConfig.BandwidthOptimization = bandwidthOptimization;
        existingConfig.ScreenSharing = screenSharing;
        existingConfig.TranscriptionService = transcriptionService;
        existingConfig.UpdatedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(existingConfig);
    }

    public async Task<bool> DeleteConfigurationAsync(string accountId)
    {
        return await _repository.DeleteByAccountIdAsync(accountId);
    }

    public async Task<IEnumerable<VideoConferencing>> GetAllConfigurationsAsync()
    {
        var queryable = await _repository.GetAllAsync();
        return queryable.ToList();
    }

    public async Task<bool> ValidateConfigurationAsync(VideoConferencing configuration)
    {
        if (string.IsNullOrEmpty(configuration.AccountId) ||
            string.IsNullOrEmpty(configuration.CalendarIntegration) ||
            string.IsNullOrEmpty(configuration.VideoConferencingProvider) ||
            string.IsNullOrEmpty(configuration.BandwidthOptimization))
        {
            return false;
        }

        return VideoConferencingProvider.SupportedProviders.Contains(configuration.VideoConferencingProvider) &&
               CalendarIntegration.SupportedIntegrations.Contains(configuration.CalendarIntegration) &&
               BandwidthOptimization.SupportedOptimizations.Contains(configuration.BandwidthOptimization);
    }
}