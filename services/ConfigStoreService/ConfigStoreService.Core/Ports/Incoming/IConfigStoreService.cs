using System.Security.Claims;
using ConfigStoreService.Core.Models;

namespace ConfigStoreService.Core.Ports.Incoming;

public interface IConfigStoreService
{
    Task<ConfigStoreResult> SavePartialConfigAsync(ClaimsPrincipal user, string accountId, string serviceType, object configData);
    Task<T?> GetConfigAsync<T>(ClaimsPrincipal user, string accountId, string serviceType) where T : class;
    Task<Configuration?> GetRawConfigAsync(ClaimsPrincipal user, string accountId, string serviceType);
    Task<ConfigStoreResult> CommitConfigAsync(ClaimsPrincipal user, string accountId, string serviceType);
    Task<IEnumerable<Configuration>> GetAllConfigurationsAsync(ClaimsPrincipal user, string accountId);
    Task<IEnumerable<Configuration>> GetConfigurationHistoryAsync(ClaimsPrincipal user, string accountId, string serviceType);
    Task<ConfigStoreResult> DeleteConfigurationAsync(ClaimsPrincipal user, string accountId, string serviceType);
    Task<bool> ValidateConfigurationAsync(string serviceType, object configData);
}

public class ConfigStoreResult
{
    public bool Success { get; set; }
    public Configuration? Configuration { get; set; }
    public string? Error { get; set; }
}