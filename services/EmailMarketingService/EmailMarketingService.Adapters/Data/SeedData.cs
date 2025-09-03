using EmailMarketingService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketingService.Adapters.Data;

public static class SeedData
{
    public static async Task Initialize(EmailMarketingDbContext context)
    {
        if (await context.EmailMarketingConfigurations.AnyAsync())
            return;

        var sampleConfig = new EmailMarketing
        {
            AccountId = "sample-account-123",
            EmailDeliveryInfra = EmailDeliveryProvider.SES,
            EmailTemplate = "welcome-template",
            Analytics = "google-analytics",
            ABTesting = "optimizely"
        };

        context.EmailMarketingConfigurations.Add(sampleConfig);
        await context.SaveChangesAsync();
    }
}