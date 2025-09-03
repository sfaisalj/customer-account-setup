using System.Security.Claims;
using IdentityService.Core.Models;
using IdentityService.Core.Ports.Outgoing;

namespace IdentityService.Adapters.Services;

public class AuthorizationService : IAuthorizationService
{
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

    public bool CanUpdateEmailMarketing(ClaimsPrincipal user)
    {
        return HasClaim(user, Claims.Customer.EmailMarketingUpdate) || 
               HasClaim(user, Claims.Support.EmailMarketingUpdate);
    }

    public bool CanUpdateVideoConferencing(ClaimsPrincipal user)
    {
        return HasClaim(user, Claims.Customer.VideoConferencingUpdate) || 
               HasClaim(user, Claims.Support.VideoConferencingUpdate);
    }

    public bool CanDeleteEmailMarketing(ClaimsPrincipal user)
    {
        return HasClaim(user, Claims.Support.EmailMarketingDelete);
    }

    public bool CanDeleteVideoConferencing(ClaimsPrincipal user)
    {
        return HasClaim(user, Claims.Support.VideoConferencingDelete);
    }
}