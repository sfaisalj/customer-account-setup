using ConfigStoreService.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ConfigStoreService.Adapters.Data;

public static class SeedData
{
    public static async Task Initialize(ConfigStoreDbContext context)
    {
        if (await context.Configurations.AnyAsync())
            return;

        var emailMarketingConfig = new
        {
            EmailDeliveryInfra = "SES",
            EmailTemplate = "welcome-template",
            Analytics = "google-analytics",
            ABTesting = "optimizely"
        };

        var videoConferencingConfig = new
        {
            CalendarIntegration = "GoogleCalendar",
            VideoConferencingProvider = "Zoom",
            BandwidthOptimization = "Auto",
            ScreenSharing = true,
            TranscriptionService = false
        };

        var sampleConfigs = new[]
        {
            new Configuration
            {
                AccountId = "sample-account-123",
                ServiceType = ServiceType.EmailMarketing,
                ConfigData = JsonSerializer.Serialize(emailMarketingConfig, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                UserId = "user-123",
                IsCommitted = true,
                Version = 1
            },
            new Configuration
            {
                AccountId = "sample-account-123",
                ServiceType = ServiceType.VideoConferencing,
                ConfigData = JsonSerializer.Serialize(videoConferencingConfig, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                UserId = "user-123",
                IsCommitted = true,
                Version = 1
            }
        };

        context.Configurations.AddRange(sampleConfigs);
        await context.SaveChangesAsync();
    }
}