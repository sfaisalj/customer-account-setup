using ConfigStoreService.Core.Models;
using ConfigStoreService.Core.Ports.Incoming;
using System.Security.Claims;

namespace ConfigStoreService.GraphQL.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Configuration>> GetConfigurations([Service] IConfigStoreService configStoreService)
    {
        // This is a bit hacky but needed for GraphQL projection
        var repo = (ConfigStoreService.Adapters.Repositories.ConfigurationRepository)
            ((ConfigStoreService.Core.Services.ConfigStoreService)configStoreService)
            .GetType()
            .GetField("_repository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(configStoreService)!;
        
        return await repo.GetAllAsync();
    }

    public async Task<IEnumerable<Configuration>> GetMyConfigurations([Service] IConfigStoreService configStoreService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return [];

        return await configStoreService.GetAllConfigurationsAsync(claimsPrincipal, accountId);
    }

    public async Task<Configuration?> GetMyConfiguration(string serviceType, [Service] IConfigStoreService configStoreService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return null;

        return await configStoreService.GetRawConfigAsync(claimsPrincipal, accountId, serviceType);
    }

    public async Task<Configuration?> GetConfigurationByAccount(string accountId, string serviceType, [Service] IConfigStoreService configStoreService, ClaimsPrincipal claimsPrincipal)
        => await configStoreService.GetRawConfigAsync(claimsPrincipal, accountId, serviceType);

    public async Task<IEnumerable<Configuration>> GetConfigurationHistory(string serviceType, [Service] IConfigStoreService configStoreService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return [];

        return await configStoreService.GetConfigurationHistoryAsync(claimsPrincipal, accountId, serviceType);
    }
}