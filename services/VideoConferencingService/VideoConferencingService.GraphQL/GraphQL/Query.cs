using VideoConferencingService.Core.Models;
using VideoConferencingService.Core.Ports.Incoming;
using System.Security.Claims;

namespace VideoConferencingService.GraphQL.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<VideoConferencing>> GetVideoConferencingConfigurations([Service] IVideoConferencingService videoConferencingService)
        => await videoConferencingService.GetAllConfigurationsAsync();

    public async Task<VideoConferencing?> GetMyVideoConferencingConfiguration([Service] IVideoConferencingService videoConferencingService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return null;

        return await videoConferencingService.GetConfigurationByAccountIdAsync(accountId);
    }

    public async Task<VideoConferencing?> GetVideoConferencingConfigurationByAccount(string accountId, [Service] IVideoConferencingService videoConferencingService)
        => await videoConferencingService.GetConfigurationByAccountIdAsync(accountId);
}