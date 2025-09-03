using System.Security.Claims;

namespace IdentityService.Core.Ports.Outgoing;

public interface IAuthorizationService
{
    bool HasClaim(ClaimsPrincipal user, string claimType);
    bool IsInRole(ClaimsPrincipal user, string role);
    string? GetUserId(ClaimsPrincipal user);
    string? GetAccountId(ClaimsPrincipal user);
    bool CanUpdateEmailMarketing(ClaimsPrincipal user);
    bool CanUpdateVideoConferencing(ClaimsPrincipal user);
    bool CanDeleteEmailMarketing(ClaimsPrincipal user);
    bool CanDeleteVideoConferencing(ClaimsPrincipal user);
}