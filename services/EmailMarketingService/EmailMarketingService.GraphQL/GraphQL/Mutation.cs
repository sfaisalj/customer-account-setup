using EmailMarketingService.Core.Models;
using EmailMarketingService.Core.Ports.Incoming;
using System.Security.Claims;

namespace EmailMarketingService.GraphQL.GraphQL;

public class Mutation
{
    public async Task<EmailMarketingResult> CreateEmailMarketingConfiguration(
        string emailDeliveryInfra, 
        string emailTemplate, 
        string analytics, 
        string abTesting, 
        [Service] IEmailMarketingService emailMarketingService, 
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(accountId))
            {
                return new EmailMarketingResult
                {
                    Success = false,
                    Error = "Account ID not found"
                };
            }

            var configuration = await emailMarketingService.CreateConfigurationAsync(accountId, emailDeliveryInfra, emailTemplate, analytics, abTesting);
            return new EmailMarketingResult
            {
                Success = true,
                Configuration = configuration
            };
        }
        catch (Exception ex)
        {
            return new EmailMarketingResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<EmailMarketingResult> UpdateEmailMarketingConfiguration(
        string emailDeliveryInfra, 
        string emailTemplate, 
        string analytics, 
        string abTesting, 
        [Service] IEmailMarketingService emailMarketingService, 
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(accountId))
            {
                return new EmailMarketingResult
                {
                    Success = false,
                    Error = "Account ID not found"
                };
            }

            var configuration = await emailMarketingService.UpdateConfigurationAsync(accountId, emailDeliveryInfra, emailTemplate, analytics, abTesting);
            if (configuration == null)
            {
                return new EmailMarketingResult
                {
                    Success = false,
                    Error = "Configuration not found"
                };
            }

            return new EmailMarketingResult
            {
                Success = true,
                Configuration = configuration
            };
        }
        catch (Exception ex)
        {
            return new EmailMarketingResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<bool> DeleteEmailMarketingConfiguration([Service] IEmailMarketingService emailMarketingService, ClaimsPrincipal claimsPrincipal)
    {
        var accountId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(accountId))
            return false;

        return await emailMarketingService.DeleteConfigurationAsync(accountId);
    }
}

public class EmailMarketingResult
{
    public bool Success { get; set; }
    public EmailMarketing? Configuration { get; set; }
    public string? Error { get; set; }
}