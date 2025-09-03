using IdentityService.Core.Models;

namespace IdentityService.Core.Ports.Outgoing;

public interface ITokenService
{
    string GenerateToken(CustomerAccount user);
    string? ValidateToken(string token);
}