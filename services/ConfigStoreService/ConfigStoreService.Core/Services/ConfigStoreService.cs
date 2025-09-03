using System.Security.Claims;
using System.Text.Json;
using ConfigStoreService.Core.Models;
using ConfigStoreService.Core.Ports.Incoming;
using ConfigStoreService.Core.Ports.Outgoing;

namespace ConfigStoreService.Core.Services;

public class ConfigStoreService : IConfigStoreService
{
    private readonly IConfigurationRepository _repository;
    private readonly IAuthorizationService _authorizationService;

    public ConfigStoreService(IConfigurationRepository repository, IAuthorizationService authorizationService)
    {
        _repository = repository;
        _authorizationService = authorizationService;
    }

    public async Task<ConfigStoreResult> SavePartialConfigAsync(ClaimsPrincipal user, string accountId, string serviceType, object configData)
    {
        if (!ServiceType.SupportedServiceTypes.Contains(serviceType))
        {
            return new ConfigStoreResult { Success = false, Error = $"Unsupported service type: {serviceType}" };
        }

        if (!_authorizationService.CanUpdateConfiguration(user, serviceType))
        {
            return new ConfigStoreResult { Success = false, Error = "Insufficient permissions to update configuration" };
        }

        var userId = _authorizationService.GetUserId(user);
        if (string.IsNullOrEmpty(userId))
        {
            return new ConfigStoreResult { Success = false, Error = "User not authenticated" };
        }

        var existingConfig = await _repository.GetByAccountAndServiceAsync(accountId, serviceType, onlyCommitted: false);
        
        var configJson = JsonSerializer.Serialize(configData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        try
        {
            Configuration result;
            if (existingConfig == null)
            {
                var newConfig = new Configuration
                {
                    AccountId = accountId,
                    ServiceType = serviceType,
                    ConfigData = configJson,
                    UserId = userId,
                    IsCommitted = false,
                    Version = 1
                };

                result = await _repository.CreateAsync(newConfig);
            }
            else
            {
                existingConfig.ConfigData = configJson;
                existingConfig.UserId = userId;
                existingConfig.UpdatedAt = DateTime.UtcNow;
                existingConfig.IsCommitted = false;

                result = await _repository.UpdateAsync(existingConfig) ?? existingConfig;
            }
            
            return new ConfigStoreResult { Success = true, Configuration = result };
        }
        catch (Exception ex)
        {
            return new ConfigStoreResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<T?> GetConfigAsync<T>(ClaimsPrincipal user, string accountId, string serviceType) where T : class
    {
        var config = await _repository.GetByAccountAndServiceAsync(accountId, serviceType, onlyCommitted: true);
        if (config == null || string.IsNullOrEmpty(config.ConfigData))
            return null;

        try
        {
            return JsonSerializer.Deserialize<T>(config.ConfigData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task<Configuration?> GetRawConfigAsync(ClaimsPrincipal user, string accountId, string serviceType)
    {
        return await _repository.GetByAccountAndServiceAsync(accountId, serviceType, onlyCommitted: false);
    }

    public async Task<ConfigStoreResult> CommitConfigAsync(ClaimsPrincipal user, string accountId, string serviceType)
    {
        if (!_authorizationService.CanUpdateConfiguration(user, serviceType))
        {
            return new ConfigStoreResult { Success = false, Error = "Insufficient permissions to commit configuration" };
        }

        var userId = _authorizationService.GetUserId(user);
        if (string.IsNullOrEmpty(userId))
        {
            return new ConfigStoreResult { Success = false, Error = "User not authenticated" };
        }

        var config = await _repository.GetByAccountAndServiceAsync(accountId, serviceType, onlyCommitted: false);
        if (config == null)
        {
            return new ConfigStoreResult { Success = false, Error = $"No configuration found for account {accountId} and service {serviceType}" };
        }

        try
        {
            config.IsCommitted = true;
            config.Version += 1;
            config.UserId = userId;
            config.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(config) ?? config;
            return new ConfigStoreResult { Success = true, Configuration = result };
        }
        catch (Exception ex)
        {
            return new ConfigStoreResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<IEnumerable<Configuration>> GetAllConfigurationsAsync(ClaimsPrincipal user, string accountId)
    {
        return await _repository.GetAllByAccountAsync(accountId);
    }

    public async Task<IEnumerable<Configuration>> GetConfigurationHistoryAsync(ClaimsPrincipal user, string accountId, string serviceType)
    {
        return await _repository.GetHistoryByAccountAndServiceAsync(accountId, serviceType);
    }

    public async Task<ConfigStoreResult> DeleteConfigurationAsync(ClaimsPrincipal user, string accountId, string serviceType)
    {
        if (!_authorizationService.CanDeleteConfiguration(user, serviceType))
        {
            return new ConfigStoreResult { Success = false, Error = "Insufficient permissions to delete configuration" };
        }

        try
        {
            var result = await _repository.DeleteByAccountAndServiceAsync(accountId, serviceType);
            return new ConfigStoreResult { Success = result, Error = result ? null : "Failed to delete configuration" };
        }
        catch (Exception ex)
        {
            return new ConfigStoreResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<bool> ValidateConfigurationAsync(string serviceType, object configData)
    {
        if (!ServiceType.SupportedServiceTypes.Contains(serviceType))
        {
            return false;
        }

        try
        {
            var json = JsonSerializer.Serialize(configData);
            return !string.IsNullOrWhiteSpace(json) && json != "{}";
        }
        catch (JsonException)
        {
            return false;
        }
    }
}