using VideoConferencingService.Core.Models;
using VideoConferencingService.Core.Ports.Incoming;
using System.Security.Claims;

namespace VideoConferencingService.GraphQL.GraphQL;

public class Mutation
{
    public async Task<VideoConferencingResult> CreateVideoConferencingConfiguration(
        string calendarIntegration,
        string videoConferencingProvider,
        string bandwidthOptimization,
        bool screenSharing,
        bool transcriptionService,
        [Service] IVideoConferencingService videoConferencingService,
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(accountId))
            {
                return new VideoConferencingResult
                {
                    Success = false,
                    Error = "Account ID not found"
                };
            }

            var configuration = await videoConferencingService.CreateConfigurationAsync(accountId, calendarIntegration, videoConferencingProvider, bandwidthOptimization, screenSharing, transcriptionService);
            return new VideoConferencingResult
            {
                Success = true,
                Configuration = configuration
            };
        }
        catch (Exception ex)
        {
            return new VideoConferencingResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<VideoConferencingResult> UpdateVideoConferencingConfiguration(
        string calendarIntegration,
        string videoConferencingProvider,
        string bandwidthOptimization,
        bool screenSharing,
        bool transcriptionService,
        [Service] IVideoConferencingService videoConferencingService,
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(accountId))
            {
                return new VideoConferencingResult
                {
                    Success = false,
                    Error = "Account ID not found"
                };
            }

            var configuration = await videoConferencingService.UpdateConfigurationAsync(accountId, calendarIntegration, videoConferencingProvider, bandwidthOptimization, screenSharing, transcriptionService);
            if (configuration == null)
            {
                return new VideoConferencingResult
                {
                    Success = false,
                    Error = "Configuration not found"
                };
            }

            return new VideoConferencingResult
            {
                Success = true,
                Configuration = configuration
            };
        }
        catch (Exception ex)
        {
            return new VideoConferencingResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<bool> DeleteVideoConferencingConfiguration([Service] IVideoConferencingService videoConferencingService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return false;

        return await videoConferencingService.DeleteConfigurationAsync(accountId);
    }
}

public class VideoConferencingResult
{
    public bool Success { get; set; }
    public VideoConferencing? Configuration { get; set; }
    public string? Error { get; set; }
}