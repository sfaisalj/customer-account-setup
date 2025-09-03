using VideoConferencingService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoConferencingService.Adapters.Data;

public static class SeedData
{
    public static async Task Initialize(VideoConferencingDbContext context)
    {
        if (await context.VideoConferencingConfigurations.AnyAsync())
            return;

        var sampleConfig = new VideoConferencing
        {
            AccountId = "sample-account-123",
            CalendarIntegration = CalendarIntegration.GoogleCalendar,
            VideoConferencingProvider = VideoConferencingProvider.Zoom,
            BandwidthOptimization = BandwidthOptimization.Auto,
            ScreenSharing = true,
            TranscriptionService = false
        };

        context.VideoConferencingConfigurations.Add(sampleConfig);
        await context.SaveChangesAsync();
    }
}