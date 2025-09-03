using IdentityService.Core.Models;
using IdentityService.Core.Ports.Incoming;

namespace IdentityService.GraphQL.GraphQL;

public class Mutation
{
    public async Task<AuthenticationResult> Login(string email, string password, [Service] IAuthenticationService authService)
    {
        return await authService.AuthenticateAsync(email, password);
    }

    public async Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password, [Service] IAuthenticationService authService)
    {
        return await authService.RegisterAsync(firstName, lastName, email, password);
    }
}