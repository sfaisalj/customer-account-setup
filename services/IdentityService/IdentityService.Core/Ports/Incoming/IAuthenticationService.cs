using IdentityService.Core.Models;

namespace IdentityService.Core.Ports.Incoming;

public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(string email, string password);
    Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password, UserRole role = UserRole.Customer);
    Task<CustomerAccount?> GetUserByIdAsync(string userId);
}

public class AuthenticationResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public CustomerAccount? User { get; set; }
    public string? Error { get; set; }
}