using System.Security.Claims;

namespace ConfigStoreService.Core.Ports.Outgoing;

public interface IAuthorizationService
{
    bool HasClaim(ClaimsPrincipal user, string claimType);
    bool IsInRole(ClaimsPrincipal user, string role);
    string? GetUserId(ClaimsPrincipal user);
    string? GetAccountId(ClaimsPrincipal user);
    bool CanUpdateConfiguration(ClaimsPrincipal user, string serviceType);
    bool CanDeleteConfiguration(ClaimsPrincipal user, string serviceType);
}