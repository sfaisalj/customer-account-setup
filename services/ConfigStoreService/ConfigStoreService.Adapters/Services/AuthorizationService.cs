using System.Security.Claims;
using ConfigStoreService.Core.Ports.Outgoing;

namespace ConfigStoreService.Adapters.Services;

public class AuthorizationService : IAuthorizationService
{
    private static class Claims
    {
        public static class Customer
        {
            public const string EmailMarketingUpdate = "EmailMarketing.Update";
            public const string VideoConferencingUpdate = "VideoConferencing.Update";
        }
        
        public static class Support
        {
            public const string EmailMarketingUpdate = "EmailMarketing.Update";
            public const string VideoConferencingUpdate = "VideoConferencing.Update";
            public const string EmailMarketingDelete = "EmailMarketing.Delete";
            public const string VideoConferencingDelete = "VideoConferencing.Delete";
        }
    }

    public bool HasClaim(ClaimsPrincipal user, string claimType)
    {
        return user?.HasClaim(claimType, "true") == true;
    }

    public bool IsInRole(ClaimsPrincipal user, string role)
    {
        return user?.IsInRole(role) == true;
    }

    public string? GetUserId(ClaimsPrincipal user)
    {
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetAccountId(ClaimsPrincipal user)
    {
        return user?.FindFirst("AccountId")?.Value;
    }

    public bool CanUpdateConfiguration(ClaimsPrincipal user, string serviceType)
    {
        return serviceType?.ToLowerInvariant() switch
        {
            "emailmarketing" => HasClaim(user, Claims.Customer.EmailMarketingUpdate) || 
                               HasClaim(user, Claims.Support.EmailMarketingUpdate),
            "videoconferencing" => HasClaim(user, Claims.Customer.VideoConferencingUpdate) || 
                                  HasClaim(user, Claims.Support.VideoConferencingUpdate),
            _ => false
        };
    }

    public bool CanDeleteConfiguration(ClaimsPrincipal user, string serviceType)
    {
        return serviceType?.ToLowerInvariant() switch
        {
            "emailmarketing" => HasClaim(user, Claims.Support.EmailMarketingDelete),
            "videoconferencing" => HasClaim(user, Claims.Support.VideoConferencingDelete),
            _ => false
        };
    }
}