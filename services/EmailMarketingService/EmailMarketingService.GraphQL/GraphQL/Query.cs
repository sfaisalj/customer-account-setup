using EmailMarketingService.Core.Models;
using EmailMarketingService.Core.Ports.Incoming;
using System.Security.Claims;

namespace EmailMarketingService.GraphQL.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<EmailMarketing>> GetEmailMarketingConfigurations([Service] IEmailMarketingService emailMarketingService)
        => await emailMarketingService.GetAllConfigurationsAsync();

    public async Task<EmailMarketing?> GetMyEmailMarketingConfiguration([Service] IEmailMarketingService emailMarketingService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return null;

        return await emailMarketingService.GetConfigurationByAccountIdAsync(accountId);
    }

    public async Task<EmailMarketing?> GetEmailMarketingConfigurationByAccount(string accountId, [Service] IEmailMarketingService emailMarketingService)
        => await emailMarketingService.GetConfigurationByAccountIdAsync(accountId);
}