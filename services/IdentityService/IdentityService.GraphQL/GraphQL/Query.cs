using IdentityService.Core.Models;
using IdentityService.Core.Ports.Incoming;
using IdentityService.Core.Ports.Outgoing;
using HotChocolate.Authorization;
using System.Security.Claims;

namespace IdentityService.GraphQL.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<CustomerAccount>> GetCustomerAccounts([Service] IUserRepository userRepository)
        => await userRepository.GetAllAsync();

    [Authorize]
    public async Task<CustomerAccount?> GetMe([Service] IAuthenticationService authService, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null;

        return await authService.GetUserByIdAsync(userId);
    }
}