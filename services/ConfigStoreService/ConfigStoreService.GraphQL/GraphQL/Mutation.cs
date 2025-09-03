using ConfigStoreService.Core.Models;
using ConfigStoreService.Core.Ports.Incoming;
using System.Security.Claims;
using System.Text.Json;

namespace ConfigStoreService.GraphQL.GraphQL;

public class Mutation
{
    public async Task<ConfigStoreResult> SavePartialConfig(
        string serviceType,
        string configDataJson,
        [Service] IConfigStoreService configStoreService,
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(userId))
            {
                return new ConfigStoreResult
                {
                    Success = false,
                    Error = "Account ID or User ID not found"
                };
            }

            // Parse the JSON to validate it and convert to object
            var configData = JsonSerializer.Deserialize<object>(configDataJson);
            
            var result = await configStoreService.SavePartialConfigAsync(claimsPrincipal, accountId, serviceType, configData);
            return result;
        }
        catch (Exception ex)
        {
            return new ConfigStoreResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ConfigStoreResult> CommitConfig(
        string serviceType,
        [Service] IConfigStoreService configStoreService,
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(userId))
            {
                return new ConfigStoreResult
                {
                    Success = false,
                    Error = "Account ID or User ID not found"
                };
            }

            var result = await configStoreService.CommitConfigAsync(claimsPrincipal, accountId, serviceType);
            return result;
        }
        catch (Exception ex)
        {
            return new ConfigStoreResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<bool> DeleteConfiguration(
        string serviceType,
        [Service] IConfigStoreService configStoreService,
        ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return false;

        var result = await configStoreService.DeleteConfigurationAsync(claimsPrincipal, accountId, serviceType);
        return result.Success;
    }
}

